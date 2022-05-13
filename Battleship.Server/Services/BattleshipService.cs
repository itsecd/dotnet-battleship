using System;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Core;

namespace Battleship.Server.Services
{
    public class BattleshipService : Api.BattleshipService.BattleshipServiceBase
    {
        public override async Task Connect(
            IAsyncStreamReader<Request> requestStream,
            IServerStreamWriter<Event> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                Console.WriteLine(requestStream.Current);
            }
        }
    }
}
