using System.Collections.Generic;
using System.Threading.Tasks;

using Battleship.Client.Models;

namespace Battleship.Client.ViewModels
{
    public class PlaygroundViewModel : ViewModelBase
    {
        public IReadOnlyList<IReadOnlyList<ICell>> Cells { get; }

        public PlaygroundViewModel() : this(Playground.Create<Cell>())
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                Cells[1][1].State = CellState.Untouched;
                Cells[1][2].State = CellState.Miss;
                Cells[1][3].State = CellState.PartialHit;
                Cells[1][4].State = CellState.CompleteHit;
            });
        }

        public PlaygroundViewModel(Playground playground)
        {
            Cells = playground.Cells;
        }
    }
}
