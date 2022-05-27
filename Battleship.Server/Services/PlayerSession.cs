using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using Battleship.Api;

using Grpc.Core;

namespace Battleship.Server.Services
{
    public sealed class PlayerSession
    {
        public string Login { get; }

        public GameSession? Session { get; set; }

        public PlayerSession(IServerStreamWriter<Event> responseStream, string login)
        {
            _responseStream = responseStream;

            Login = login;

            Task.Run(SendThread);
        }

        public void Send(Event @event)
        {
            _responseQueue.Post(@event);
        }

        private async Task SendThread()
        {
            try
            {
                while (await _responseQueue.OutputAvailableAsync())
                {
                    await _responseStream.WriteAsync(_responseQueue.Receive());
                }
            }
            catch
            {
                _responseQueue.Complete();
            }
        }

        private readonly IServerStreamWriter<Event> _responseStream;
        private readonly BufferBlock<Event> _responseQueue = new();
    }
}
