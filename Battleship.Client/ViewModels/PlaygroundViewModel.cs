using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;

namespace Battleship.Client.ViewModels
{
    public class PlaygroundViewModel : BaseViewModel
    {
        public ReactiveCommand<ICell, Unit> CellClicked { get; }

        public IReadOnlyList<IReadOnlyList<ICell>> Cells { get; }

        public PlaygroundViewModel() : this(Playground.Create<CellViewModel>())
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                Cells[1][1].State = CellState.NotDestroyed;
                Cells[1][2].State = CellState.Missed;
                Cells[1][3].State = CellState.PartiallyDestroyed;
                Cells[1][4].State = CellState.CompletelyDestroyed;
            });
        }

        public PlaygroundViewModel(Playground playground)
        {
            Cells = playground.Cells;

            CellClicked = ReactiveCommand.CreateFromTask<ICell, Unit>(CellClickedImpl);
        }

        protected virtual Task<Unit> CellClickedImpl(ICell cell)
        {
            Console.WriteLine(cell);
            return Task.FromResult(Unit.Default);
        }
    }
}
