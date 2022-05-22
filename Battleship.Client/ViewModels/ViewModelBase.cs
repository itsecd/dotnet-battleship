using System.Reactive;

using ReactiveUI;

namespace Battleship.Client.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public Interaction<string, Unit> ShowErrorInteraction { get; } = new();
    }
}
