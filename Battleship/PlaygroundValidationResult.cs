namespace Battleship
{
    public sealed class PlaygroundValidationResult
    {
        public int ActualCorvetteCount { get; internal set; }
        public int ActualDestroyerCount { get; internal set; }
        public int ActualCruiserCount { get; internal set; }
        public int ActualBattleshipCount { get; internal set; }

        public bool IsNeighborhoodValid { get; internal set; }

        public bool IsOnlyValidShips { get; internal set; }
    }
}
