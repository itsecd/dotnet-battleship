using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Battleship.Api;

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
            var canLogin = this
                .WhenAnyValue(
                    o => o.ServerAddress,
                    o => o.Login,
                    (serverAddress, login) =>
                        !string.IsNullOrWhiteSpace(serverAddress) &&
                        !string.IsNullOrWhiteSpace(login));

            LoginCommand = ReactiveCommand.CreateFromTask(LoginImpl, canLogin);
        }

        private async Task<Unit> LoginImpl()
        {
            try
            {
                var connection = new Connection($"{ServerAddress}:{ServerPort}");
                connection.LoginEvent.Subscribe(loginEvent => Console.WriteLine(loginEvent.Success));
                connection.DisconnectedEvent.Subscribe(unit => Console.WriteLine("Connection failed"));
                await connection.LoginRequest(Login);
                return Unit.Default;
            }
            catch
            {
                Console.WriteLine("Connection failed");
                return Unit.Default;
            }
        }
    }
}
