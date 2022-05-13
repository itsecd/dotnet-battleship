using System.Collections.Generic;

using Battleship.Client.Models;

namespace Battleship.Client.ViewModels
{
    public class PlaygroundViewModel : ViewModelBase
    {
        public IReadOnlyList<IReadOnlyList<ICell>> Cells { get; }

        public PlaygroundViewModel() : this(new Playground<ReactiveCell>())
        {
        }

        public PlaygroundViewModel(Playground<ReactiveCell> playground)
        {
            Cells = playground.Cells;
        }
    }
}
