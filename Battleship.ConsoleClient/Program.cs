using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Battleship.Api;

using Grpc.Net.Client;

namespace Battleship.ConsoleClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new BattleshipService.BattleshipServiceClient(channel);
            using var stream = client.Connect();

            var task = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                }
            });

            await stream.RequestStream.WriteAsync(CreateLogin("Ivan"));
            await stream.RequestStream.WriteAsync(CreateFindOpponent());
            await stream.RequestStream.CompleteAsync();
            await task;
        }

        private static Request CreateLogin(string login)
        {
            var loginRequest = new LoginRequest {Login = login};
            return new Request {Login = loginRequest};
        }

        private static Request CreateFindOpponent()
        {
            var findOpponentRequest = new FindOpponentRequest();
            return new Request {FindOpponent = findOpponentRequest};
        }
    }
}
