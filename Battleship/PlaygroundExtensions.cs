using System;
using System.Collections.Generic;

namespace Battleship
{
    public static class PlaygroundExtensions
    {
        private const CellState EmptyCell = CellState.None;
        private const CellState BusyCell = CellState.NotDestroyed;

        public static PlaygroundValidationResult Validate(this Playground playground)
        {
            var processed = new HashSet<(int, int)>();
            var validationResult = new PlaygroundValidationResult
            {
                IsOnlyValidShips = true,
                IsNeighborhoodValid = true
            };

            for (var i = 0; i < playground.Height; ++i)
            for (var j = 0; j < playground.Width; ++j)
            {
                var state = playground[i, j];
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (state)
                {
                    case EmptyCell:
                        continue;
                    case BusyCell:
                        Track(playground, i, j, processed, validationResult);
                        continue;
                    default:
                        throw new InvalidOperationException();
                }
            }

            return validationResult;
        }

        private static void Track(
            Playground playground,
            int i, int j,
            ISet<(int, int)> processed,
            PlaygroundValidationResult validationResult
        )
        {
            var size = 0;
            var di = 0;
            var dj = 0;
            while (playground[i, j] == BusyCell)
            {
                processed.Add((i, j));
                ++size;

                if (!CheckNeighborhood(playground, i, j))
                    validationResult.IsNeighborhoodValid = false;

                if ((di, dj) == (0, 0))
                {
                    var directionCount = 0;
                    if (playground[i + 1, j] == BusyCell)
                        (di, dj, directionCount) = (+1, 0, directionCount + 1);
                    if (playground[i, j + 1] == BusyCell)
                        (di, dj, directionCount) = (0, +1, directionCount + 1);

                    if (directionCount == 0)
                        break;

                    if (directionCount == 1)
                        continue;

                    validationResult.IsOnlyValidShips = false;
                    return;
                }
                else
                {
                    i += di;
                    j += dj;
                }
            }

            switch (size)
            {
                case 1:
                    ++validationResult.ActualCorvetteCount;
                    break;
                case 2:
                    ++validationResult.ActualDestroyerCount;
                    break;
                case 3:
                    ++validationResult.ActualCruiserCount;
                    break;
                case 4:
                    ++validationResult.ActualBattleshipCount;
                    break;
                default:
                    validationResult.IsOnlyValidShips = false;
                    break;
            }
        }

        private static bool CheckNeighborhood(Playground playground, int i, int j)
        {
            return
                playground[i - 1, j - 1] == EmptyCell &&
                playground[i - 1, j + 1] == EmptyCell &&
                playground[i + 1, j - 1] == EmptyCell &&
                playground[i + 1, j + 1] == EmptyCell;
        }
    }
}
