using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Battleship.Client.Models
{
    public class ReactiveCell : ReactiveObject, ICell
    {
        [Reactive]
        public CellState State { get; set; }
    }
}
