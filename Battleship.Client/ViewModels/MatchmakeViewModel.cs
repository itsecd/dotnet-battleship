using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Battleship.Api;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Battleship.Client.ViewModels
{
    public class MatchmakeViewModel : ClientBaseViewModel
    {
        public enum EMode
        {
            PlayButton,
            FindingOpponent
        }

        [Reactive]
        public EMode Mode { get; private set; }

        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        public Interaction<OpponentFoundEvent, Unit> ShowPrepareWindowInteraction { get; } = new();

        public MatchmakeViewModel(Client client) : base(client)
        {
            PlayCommand = ReactiveCommand.CreateFromTask(PlayImpl);

            this.WhenActivated(d =>
            {
                d(Client.OpponentFoundEvent.Subscribe(async opponentFoundEvent =>
                {
                    await ShowPrepareWindowInteraction.Handle(opponentFoundEvent);
                }));
            });
        }

        private async Task PlayImpl()
        {
            await Client.FindOpponentRequest();

            Mode = EMode.FindingOpponent;
        }
    }
}
