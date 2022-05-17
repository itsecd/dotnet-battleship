using System.Collections.Concurrent;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;

namespace Battleship.Server.Services
{
    public sealed class GameService
    {
        public async Task Connect(
            IAsyncStreamReader<Request> requestStream,
            IServerStreamWriter<Event> responseStream)
        {
            if (!await requestStream.MoveNext())
                return;

            if (requestStream.Current.RequestCase != Request.RequestOneofCase.Login)
                return;

            var session = Login(requestStream.Current.Login);
            try
            {
                var loginEvent = new LoginEvent {Success = session is not null};
                await responseStream.WriteAsync(new Event {Login = loginEvent});
                if (session is null)
                    return;

                while (await requestStream.MoveNext())
                {
                }
            }
            finally
            {
                if (session is not null)
                    _players.TryRemove(session.Login, out _);
            }
        }

        private PlayerSession? Login(LoginRequest loginRequest)
        {
            var session = new PlayerSession(loginRequest.Login);
            return _players.TryAdd(session.Login, session) ? session : null;
        }

        private readonly ConcurrentDictionary<string, PlayerSession> _players = new();
    }
}
