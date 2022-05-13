using System;
using System.Collections.Generic;

namespace Battleship
{
    public sealed class PlaygroundExtensions
    {
        public PlaygroundValidationResult Validate<T>(Playground<T> playground)
            where T : ICell, new()
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
                var state = playground.Cells[i][j].State;
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (state)
                {
                    case CellState.None:
                        continue;
                    case CellState.Untouched:
                        Track(playground, i, j, processed, validationResult);
                        continue;
                    default:
                        throw new InvalidOperationException();
                }
            }

            return validationResult;
        }

        private static void Track<T>(
            Playground<T> playground,
            int i, int j,
            HashSet<(int, int)> processed,
            PlaygroundValidationResult validationResult
        ) where T : ICell, new()
        {
            int size = 0;
            int di = 0;
            int dj = 0;
            while (playground.Cells[i][j].State == CellState.Untouched)
            {
                processed.Add((i, j));
                ++size;

                if (!CheckNeighborhood(playground, i, j))
                    validationResult.IsNeighborhoodValid = false;

                if ((di, dj) == (0, 0))
                {
                    var directionCount = 0;
                    if (playground.Cells[i + 1][j].State == CellState.Untouched)
                        (di, dj, directionCount) = (+1, 0, directionCount + 1);
                    if (playground.Cells[i][j + 1].State == CellState.Untouched)
                        (di, dj, directionCount) = (0, +1, directionCount + 1);

                    if (directionCount == 0)
                        break;

                    if (directionCount != 1)
                    {
                        validationResult.IsOnlyValidShips = false;
                        return;
                    }
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

        private static bool CheckNeighborhood<T>(Playground<T> playground, int i, int j) where T : ICell, new()
        {
            return
                playground.Cells[i - 1][j - 1].State == CellState.None &&
                playground.Cells[i - 1][j + 1].State == CellState.None &&
                playground.Cells[i + 1][j - 1].State == CellState.None &&
                playground.Cells[i + 1][j + 1].State == CellState.None;
        }
    }
}
