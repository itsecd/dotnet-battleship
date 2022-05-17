using System;
using System.Reactive;
using System.Threading.Tasks;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Battleship.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        [Reactive]
        public string ServerAddress { get; set; } = "http://127.0.0.1";

        [Reactive]
        public int ServerPort { get; set; } = 5000;

        [Reactive]
        public string Login { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = ReactiveCommand.CreateFromTask(LoginImpl);
        }

        private async Task<Unit> LoginImpl()
        {
            Console.WriteLine("LoginImpl: " + Login);
            return Unit.Default;
        }
    }
}
