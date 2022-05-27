using System;
using System.Reactive.Linq;

using ReactiveUI;

namespace Battleship.Client.ViewModels
{
    public class ClientBaseViewModel : BaseViewModel, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new();

        public Client Client { get; }

        protected ClientBaseViewModel(Client client)
        {
            Client = client;

            this.WhenActivated(d =>
            {
                d(Client.DisconnectedEvent.Subscribe(async _ => await ShowErrorInteraction.Handle("Connection failed.")));
            });
        }
    }
}
