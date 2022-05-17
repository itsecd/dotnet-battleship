using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;

namespace Battleship.Server.Services
{
    public class EntryPointService : BattleshipService.BattleshipServiceBase
    {
        private readonly GameService _gameService;

        public EntryPointService(GameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task Connect(
            IAsyncStreamReader<Request> requestStream,
            IServerStreamWriter<Event> responseStream,
            ServerCallContext context)
        {
            await _gameService.Connect(requestStream, responseStream);
        }
    }
}
