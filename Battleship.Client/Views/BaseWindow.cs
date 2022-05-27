using System.Reactive;
using System.Threading.Tasks;

using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public class BaseWindow<TViewModel> : ReactiveWindow<TViewModel> where TViewModel : class
    {
        protected BaseWindow()
        {
            this.WhenActivated(d =>
            {
                if (ViewModel is not BaseViewModel viewModel)
                    return;

                d(viewModel.ShowErrorInteraction.RegisterHandler(ShowError));
            });
        }

        private async Task ShowError(InteractionContext<string, Unit> ctx)
        {
            var messageWindow = new MessageWindow(ctx.Input);
            await messageWindow.ShowDialog(this);

            new LoginWindow
            {
                DataContext = new LoginViewModel()
            }.ShowCenter(this);

            Close();

            ctx.SetOutput(Unit.Default);
        }
    }
}
