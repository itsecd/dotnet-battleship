using System.Collections.Generic;
using System.Collections.Immutable;

namespace Battleship
{
    public class Playground<T> where T : ICell, new()
    {
        public int ExpectedCorvetteCount => 4;

        public int ExpectedDestroyerCount => 3;

        public int ExpectedCruiserCount => 2;

        public int ExpectedBattleshipCount => 1;

        public int Width => 10;

        public int Height => 10;

        public IReadOnlyList<IReadOnlyList<T>> Cells { get; }

        public Playground()
        {
            var cells = new List<IReadOnlyList<T>>(Height);
            for (var i = 0; i < Height; ++i)
            {
                var row = new List<T>(Width);
                for (var j = 0; j < Width; ++j)
                    row.Add(new T());
                cells.Add(row.ToImmutableArray());
            }

            Cells = cells.ToImmutableArray();
        }
    }
}
