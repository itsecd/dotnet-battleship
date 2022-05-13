using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

namespace Battleship.Client.Views
{
    public partial class PlaygroundView : ReactiveUserControl<PlaygroundViewModel>
    {
        public PlaygroundView()
        {
            InitializeComponent();
        }
    }
}
