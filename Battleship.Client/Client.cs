using System;
using System.Reactive;
using System.Reactive.Linq;
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

        public IObservable<LoginEvent> LoginEvent => _loginSubject.ObserveOn(RxApp.MainThreadScheduler);

        public IObservable<OpponentFoundEvent> OpponentFoundEvent => _opponentFoundSubject.ObserveOn(RxApp.MainThreadScheduler);

        public IObservable<Unit> DisconnectedEvent => _disconnectedSubject.ObserveOn(RxApp.MainThreadScheduler);

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
                var loginRequest = new LoginRequest { Login = login };
                var request = new Request { Login = loginRequest };
                await _stream.RequestStream.WriteAsync(request);
            }
            catch
            {
                OnDisconnected();
            }
        }

        public async Task FindOpponentRequest()
        {
            try
            {
                var findOpponentRequest = new FindOpponentRequest();
                var request = new Request { FindOpponent = findOpponentRequest };
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
                                System.Diagnostics.Debug.WriteLine(stream.Current.Login);
                            _loginSubject.OnNext(stream.Current.Login);
                            break;

                        case Event.EventOneofCase.OpponentFound:
                            System.Diagnostics.Debug.WriteLine(stream.Current.OpponentFound);
                            if (!_opponentFoundSubject.HasObservers)
                                System.Diagnostics.Debug.WriteLine(stream.Current.OpponentFound);
                            _opponentFoundSubject.OnNext(stream.Current.OpponentFound);
                            break;

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
            lock (_disconnectedSubject)
            {
                if (_alreadyDisconnected)
                    return;

                _disconnectedSubject.OnNext(Unit.Default);
                _alreadyDisconnected = true;
            }
        }

        private readonly GrpcChannel _channel;
        private readonly AsyncDuplexStreamingCall<Request, Event> _stream;
        private readonly Subject<LoginEvent> _loginSubject = new();
        private readonly Subject<Unit> _disconnectedSubject = new();
        private readonly Subject<OpponentFoundEvent> _opponentFoundSubject = new();
        private bool _alreadyDisconnected;
    }
}
