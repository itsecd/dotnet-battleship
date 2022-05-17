namespace Battleship.Server.Services
{
    public class PlayerSession
    {
        public string Login { get; }

        public PlayerSession(string login)
        {
            Login = login;
        }
    }
}
