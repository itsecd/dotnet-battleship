using Avalonia.ReactiveUI;

using Battleship.Client.ViewModels;

namespace Battleship.Client.Views
{
    public partial class LoginWindow : Window<LoginViewModel>
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
    }
}
