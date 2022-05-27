using Battleship.Api;

namespace Battleship.Server.Services
{
    public sealed class GameSession
    {
        public GameSession(Configuration configuration, PlayerSession firstPlayer, PlayerSession secondPlayer)
        {
            _configuration = configuration;

            _firstPlayer = firstPlayer;
            _firstPlayer.Session = this;

            _secondPlayer = secondPlayer;
            _secondPlayer.Session = this;
        }

        public void Start()
        {
            _firstPlayer.Send(CreateOpponentFoundEvent(_secondPlayer.Login));
            _secondPlayer.Send(CreateOpponentFoundEvent(_firstPlayer.Login));
        }

        private Event CreateOpponentFoundEvent(string opponentLogin)
        {
            return new Event
            {
                OpponentFound = new OpponentFoundEvent
                {
                    OpponentLogin = opponentLogin,
                    PreparationTimeoutSeconds = _configuration.PreparationTimeout
                }
            };
        }


        private readonly Configuration _configuration;
        private readonly PlayerSession _firstPlayer;
        private readonly PlayerSession _secondPlayer;
    }
}
