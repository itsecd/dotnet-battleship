namespace Battleship
{
    public static class PlaygroundExtensions
    {
        public static bool IsValid<T>(this Playground<T> playground) where T : ICell, new()
        {
            return true;
        }
    }
}
