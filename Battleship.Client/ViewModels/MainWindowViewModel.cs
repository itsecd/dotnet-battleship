namespace Battleship.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PlaygroundViewModel Edit { get; } = new();
        public PlaygroundViewModel My { get; } = new();
        public PlaygroundViewModel Opponent { get; } = new();
    }
}
