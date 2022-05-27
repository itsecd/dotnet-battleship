using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Battleship
{
    public sealed class Playground
    {
        public static Playground Create<T>() where T : ICell, new()
        {
            return new Playground(() => new T());
        }

        public int ExpectedCorvetteCount => 4;

        public int ExpectedDestroyerCount => 3;

        public int ExpectedCruiserCount => 2;

        public int ExpectedBattleshipCount => 1;

        public int Width => 10;

        public int Height => 10;

        public IReadOnlyList<IReadOnlyList<ICell>> Cells { get; }

        public CellState this[int i, int j]
        {
            get
            {
                if (i < 0 || Cells.Count <= i)
                    return CellState.None;
                if (j < 0 || Cells[i].Count <= j)
                    return CellState.None;
                return Cells[i][j].State;
            }
            set => Cells[i][j].State = value;
        }

        private Playground(Func<ICell> cellFactory)
        {
            var cells = new List<IReadOnlyList<ICell>>(Height);
            for (var i = 0; i < Height; ++i)
            {
                var row = new List<ICell>(Width);
                for (var j = 0; j < Width; ++j)
                    row.Add(cellFactory());
                cells.Add(row.ToImmutableArray());
            }

            Cells = cells.ToImmutableArray();
        }
    }
}
