using System;
using System.Linq;

using Avalonia.ReactiveUI;

using Battleship.Client.Models;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public partial class CellView : ReactiveUserControl<Cell>
    {
        // CellStateNone,
        // CellStateUntouched,
        // CellStateMiss,
        // CellStatePartialHit,
        // CellStateCompleteHit
        private const string CellStateClassPrefixName = "CellState";

        public CellView()
        {
            InitializeComponent();

            this.WhenAnyValue(o => o.ViewModel, o => o.ViewModel!.State,
                    (viewModel, _) => viewModel?.State)
                .Subscribe(OnCellStateChanged);
        }

        private void OnCellStateChanged(CellState? cellState)
        {
            var classes = CellRectangle.Classes;

            var oldClassed = classes.Where(o => o.StartsWith(CellStateClassPrefixName));
            classes.RemoveAll(oldClassed);

            if (cellState is null)
                return;

            var className = CellStateClassPrefixName + cellState.Value;
            classes.Add(className);
        }
    }
}
