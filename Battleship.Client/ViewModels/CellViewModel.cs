using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Battleship.Client.ViewModels
{
    public class CellViewModel : ReactiveObject, ICell
    {
        [Reactive]
        public CellState State { get; set; }
    }
}
