using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

namespace Battleship.Client.Views
{
    public partial class LoginWindow : ReactiveWindow<LoginViewModel>
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
    }
}
