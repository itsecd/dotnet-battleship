using System.Reactive;

using Battleship.Client.ViewModels;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public partial class LoginWindow : BaseWindow<LoginViewModel>
    {
        public LoginWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {

                if (ViewModel is not { } viewModel)
                    return;

                d(viewModel.ShowMatchmakeWindowInteraction.RegisterHandler(ShowMatchmakeWindowImpl));
            });
        }

        private void ShowMatchmakeWindowImpl(InteractionContext<Client, Unit> ctx)
        {
            var viewModel = new MatchmakeViewModel(ctx.Input);
            var window = new MatchmakeWindow { ViewModel = viewModel };
            window.ShowCenter(this);

            Close();

            ctx.SetOutput(Unit.Default);
        }
    }
}
