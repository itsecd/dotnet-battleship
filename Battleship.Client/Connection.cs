using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;
using Grpc.Net.Client;

namespace Battleship.Client
{
    public sealed class Connection
    {
        public string Login { get; private set; } = string.Empty;

        public IObservable<LoginEvent> LoginEvent => _loginSubject;

        public IObservable<Unit> DisconnectedEvent => _disconnectedSubject;

        public Connection(string address)
        {
            var channel = GrpcChannel.ForAddress(address);
            var client = new BattleshipService.BattleshipServiceClient(channel);
            _stream = client.Connect();
            Task.Run(ReadEvents);
        }

        public async Task LoginRequest(string login)
        {
            Login = login;
            var loginRequest = new LoginRequest {Login = login};
            var request = new Request {Login = loginRequest};
            await _stream.RequestStream.WriteAsync(request);
        }

        private async Task ReadEvents()
        {
            try
            {
                var stream = _stream.ResponseStream;
                while (await stream.MoveNext(CancellationToken.None))
                {
                    switch (stream.Current.EventCase)
                    {
                        case Event.EventOneofCase.None:
                            throw new InvalidOperationException();
                        case Event.EventOneofCase.Login:
                            if (!_loginSubject.HasObservers)
                                Console.WriteLine(stream.Current.Login);
                            _loginSubject.OnNext(stream.Current.Login);
                            break;
                        case Event.EventOneofCase.OpponentFound:
                            throw new InvalidOperationException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch
            {
                _disconnectedSubject.OnNext(Unit.Default);
            }
        }

        private readonly AsyncDuplexStreamingCall<Request, Event> _stream;
        private readonly Subject<LoginEvent> _loginSubject = new();
        private readonly Subject<Unit> _disconnectedSubject = new();
    }
}
