using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

namespace Battleship.Client.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
