using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Battleship.Client
{
    public static class WindowExtensions
    {
        public static void ShowCenter(this Window window, Window parentWindow)
        {
            if (parentWindow.Screens.ScreenFromWindow(parentWindow.PlatformImpl) is not Screen screen)
            {
                window.Show();
                return;
            }

            var parentSize = parentWindow.Bounds.Size / 2;
            var size = window.DesiredSize / 2;
            var shift = new Point(parentSize.Width - size.Width, parentSize.Height - size.Height) / 2;
            var screenShift = PixelPoint.FromPointWithDpi(shift, screen.PixelDensity);

            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Position = parentWindow.Position + screenShift;

            window.Show();
        }
    }
}
