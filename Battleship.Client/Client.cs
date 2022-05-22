using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;
using Grpc.Net.Client;

using ReactiveUI;

namespace Battleship.Client
{
    public sealed class Client : IDisposable
    {
        public string Login { get; private set; } = string.Empty;

        public IObservable<LoginEvent> LoginEvent => _loginSubject;

        public IObservable<Unit> DisconnectedEvent => _disconnectedSubject;

        public Client(string address)
        {
            _channel = GrpcChannel.ForAddress(address);
            var client = new BattleshipService.BattleshipServiceClient(_channel);
            _stream = client.Connect();
            Task.Run(ReadEvents);
        }

        public async Task LoginRequest(string login)
        {
            try
            {
                Login = login;
                var loginRequest = new LoginRequest {Login = login};
                var request = new Request {Login = loginRequest};
                await _stream.RequestStream.WriteAsync(request);
            }
            catch
            {
                OnDisconnected();
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
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
                OnDisconnected();
            }
        }

        private void OnDisconnected()
        {
            RxApp.MainThreadScheduler.Schedule(Unit.Default, (scheduler, state) =>
            {
                if (_alreadyDisconnected)
                    return Disposable.Empty;

                _disconnectedSubject.OnNext(Unit.Default);
                _alreadyDisconnected = true;

                return Disposable.Empty;
            });
        }

        private readonly GrpcChannel _channel;
        private readonly AsyncDuplexStreamingCall<Request, Event> _stream;
        private readonly Subject<LoginEvent> _loginSubject = new();
        private readonly Subject<Unit> _disconnectedSubject = new();
        private bool _alreadyDisconnected;
    }
}
