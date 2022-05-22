using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Hosting;

using Timer = System.Timers.Timer;

namespace Battleship.Server.Services
{
    public class MatchmakingService : IHostedService, IMatchmakingService
    {
        public MatchmakingService(Configuration configuraion)
        {
            _configuration = configuraion;
        }

        public void Enqueue(PlayerSession playerSession)
        {
            lock (_awaitingPlayers)
            {
                _awaitingPlayers.Enqueue(playerSession);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer {Interval = _configuration.MatchmakingInterval};
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            _timer = null;

            return Task.CompletedTask;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick");

            lock (_awaitingPlayers)
            {
                while (_awaitingPlayers.Count > 1)
                {
                    _awaitingPlayers.Dequeue();
                    _awaitingPlayers.Dequeue();
                }
            }
        }

        private readonly Configuration _configuration;
        private readonly Queue<PlayerSession> _awaitingPlayers = new();
        private Timer? _timer;
    }
}
