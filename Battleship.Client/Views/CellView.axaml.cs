using System;
using System.Linq;

using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

using ReactiveUI;

namespace Battleship.Client.Views
{
    public partial class CellView : ReactiveUserControl<CellViewModel>
    {
        // cell-state-none,
        // cell-state-missed,
        // cell-state-not-destroyed,
        // cell-state-partially-destroyed,
        // cell-state-completely-destroyed
        private const string CellStateClassPrefixName = "cell-state";

        public CellView()
        {
            InitializeComponent();

            this.WhenAnyValue(o => o.ViewModel, o => o.ViewModel!.State,
                    (viewModel, _) => viewModel?.State)
                .Subscribe(OnCellStateChanged);
        }

        private void OnCellStateChanged(CellState? cellState)
        {
            var classes = CellImage.Classes;

            var oldClassed = classes.Where(o => o.StartsWith(CellStateClassPrefixName));
            classes.RemoveAll(oldClassed);

            if (cellState is null)
                return;

            var className = CellStateClassPrefixName + PascalCaseToKebabCase(cellState.Value.ToString());
            classes.Add(className);
        }

        private static string PascalCaseToKebabCase(string name)
        {
            var strings = name.Select(c => char.IsUpper(c) ? "-" + char.ToLower(c) : c.ToString());
            return string.Concat(strings);
        }
    }
}
