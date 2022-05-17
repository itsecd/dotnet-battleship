using System;

using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;

using Battleship.Client.ViewModels;

namespace Battleship.Client.Views
{
    public partial class PlaygroundView : ReactiveUserControl<PlaygroundViewModel>
    {
        public PlaygroundView()
        {
            InitializeComponent();

            AddHandler(PointerPressedEvent, OnPressed, RoutingStrategies.Tunnel);
        }

        private void OnPressed(object? sender, PointerPressedEventArgs e)
        {
            if (ViewModel is not { } viewModel ||
                e.Source is not IVisual visual)
                return;

            var cellView = visual.FindAncestorOfType<CellView?>();

            if (cellView?.ViewModel is not { } cell)
                return;

            viewModel.CellClicked
                .Execute(cell)
                .Subscribe();
        }
    }
}
