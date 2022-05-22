using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public class Window<TViewModel> : ReactiveWindow<TViewModel> where TViewModel : class
    {
        protected Window()
        {
            this.WhenActivated((CompositeDisposable cd) =>
            {
                if (ViewModel is not ViewModelBase viewModel)
                    return;

                var d = viewModel.ShowErrorInteraction.RegisterHandler(ShowError);
                cd.Add(d);
            });
        }

        private async Task ShowError(InteractionContext<string, Unit> ctx)
        {
            var messageWindow = new MessageWindow(ctx.Input);
            await messageWindow.ShowDialog(this);
            ctx.SetOutput(Unit.Default);
        }
    }
}
