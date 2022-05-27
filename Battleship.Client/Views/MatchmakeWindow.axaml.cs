using System.Reactive;

using Battleship.Api;
using Battleship.Client.ViewModels;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public partial class MatchmakeWindow : BaseWindow<MatchmakeViewModel>
    {
        public MatchmakeWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                if (ViewModel is not { } viewModel)
                    return;

                d(viewModel.ShowPrepareWindowInteraction.RegisterHandler(ShowPrepareWindowImpl));
            });
        }

        private void ShowPrepareWindowImpl(InteractionContext<OpponentFoundEvent, Unit> ctx)
        {
            System.Diagnostics.Debug.WriteLine(nameof(ShowPrepareWindowImpl));

            ctx.SetOutput(Unit.Default);
        }
    }
}
