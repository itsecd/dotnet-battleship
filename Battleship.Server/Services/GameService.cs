using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;

namespace Battleship.Server.Services
{
    public sealed class GameService
    {
        public GameService(IMatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        public async Task Connect(
            IAsyncStreamReader<Request> requestStream,
            IServerStreamWriter<Event> responseStream)
        {
            if (!await requestStream.MoveNext())
                return;

            if (requestStream.Current.RequestCase != Request.RequestOneofCase.Login)
                return;

            var playerSession = Login(responseStream, requestStream.Current.Login);
            try
            {
                var loginEvent = new LoginEvent { Success = playerSession is not null };
                await responseStream.WriteAsync(new Event { Login = loginEvent });
                if (playerSession is null)
                    return;

                while (await requestStream.MoveNext())
                {
                    DispatchRequest(playerSession, requestStream.Current);
                }
            }
            finally
            {
                if (playerSession is not null)
                    _players.TryRemove(playerSession.Login, out _);
            }
        }

        private void DispatchRequest(PlayerSession playerSession, Request request)
        {
            switch (request.RequestCase)
            {
                case Request.RequestOneofCase.None:
                    throw new InvalidOperationException();
                case Request.RequestOneofCase.Login:
                    throw new InvalidOperationException();
                case Request.RequestOneofCase.FindOpponent:
                    _matchmakingService.Enqueue(playerSession);
                    break;
                case Request.RequestOneofCase.PreparePlayground:
                    throw new NotImplementedException();
                case Request.RequestOneofCase.MakeTurn:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(request));
            }
        }

        private PlayerSession? Login(IServerStreamWriter<Event> responseStream, LoginRequest loginRequest)
        {
            var session = new PlayerSession(responseStream, loginRequest.Login);
            return _players.TryAdd(session.Login, session) ? session : null;
        }

        private readonly IMatchmakingService _matchmakingService;
        private readonly ConcurrentDictionary<string, PlayerSession> _players = new();
    }
}
