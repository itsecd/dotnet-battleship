namespace Battleship.Server.Services
{
    public interface IMatchmakingService
    {
        void Enqueue(PlayerSession playerSession);
    }
}
