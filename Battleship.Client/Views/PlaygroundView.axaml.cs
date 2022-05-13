using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

using ReactiveUI;

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
