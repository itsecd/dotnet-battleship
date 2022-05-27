using System.Reactive;

using ReactiveUI;

namespace Battleship.Client.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        public Interaction<string, Unit> ShowErrorInteraction { get; } = new();
    }
}
