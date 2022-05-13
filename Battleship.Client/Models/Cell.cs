using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Battleship.Client.Models
{
    public class Cell : ReactiveObject, ICell
    {
        [Reactive]
        public CellState State { get; set; }
    }
}
