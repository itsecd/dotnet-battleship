using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Battleship.Client.Views
{
    public partial class MessageWindow : Window
    {
        public MessageWindow() : this(string.Empty)
        {
        }

        public MessageWindow(string message)
        {
            InitializeComponent();

            Message.Text = message;
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
