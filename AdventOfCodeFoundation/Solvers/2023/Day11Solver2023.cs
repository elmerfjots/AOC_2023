using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/11")]
    internal class Day11Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").ToList();
            var pairs = GetPairs(GetGalaxies(rows));
            long sum = 0;
            foreach (var (g1, g2) in pairs)
            {
                sum += g1.ShortestDistanceTo(g2);
            }
            return sum.ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").ToList();
            var pairs = GetPairs(GetGalaxies(rows, 1000000));
            long sum = 0;
            foreach (var (g1, g2) in pairs)
            {
                sum += g1.ShortestDistanceTo(g2);
            }
            return sum.ToString();
        }
        private HashSet<(Galaxy, Galaxy)> GetPairs(List<Galaxy> galaxies)
        {
            var pairs = new HashSet<(Galaxy, Galaxy)>();
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    if (j < galaxies.Count)
                        pairs.Add((galaxies[i], galaxies[j]));
                }
            }
            return pairs;
        }
        private List<Galaxy> GetGalaxies(List<string> rows, long expansionMultiplier = 2)
        {

            var galaxies = new List<Galaxy>();
            var rowColIndexes = GetRowColIndexes(rows);

            for (var row = 0; row < rows.Count; row++)
            {
                var rowOffset = rowColIndexes.rowIndexes.Count(idx => idx <= row) * (expansionMultiplier - 1);
                for (var col = 0; col < rows[0].Length; col++)
                {
                    if (rows[row][col] != '#') continue;

                    var colOffset = rowColIndexes.colIndexes.Count(idx => idx <= col) * (expansionMultiplier - 1);
                    galaxies.Add(new Galaxy(row + rowOffset, col + colOffset));
                }
            }
            return galaxies;
        }

        private (int[] rowIndexes, int[] colIndexes) GetRowColIndexes(List<string> rows)
        {
            List<int> indexes = new List<int>();

            for (var i = 0; i < rows.Count; i++)
            {
                if (rows[i].All(c => c.Equals('.'))) { indexes.Add(i); }
            }
            var rIdxs = indexes.ToArray();
            indexes.Clear();
            for (var i = 0; i < rows[0].Length; i++)
            {
                if (rows.All(col => col[i].Equals('.'))) { indexes.Add(i); }
            }

            return (rIdxs, indexes.ToArray());
        }

        class Galaxy
        {
            public (long row, long col) Position { get; set; }
            public long Number { get; set; }
            public Galaxy(long row, long col)
            {
                Position = (row, col);
            }
            public override int GetHashCode()
            {
                int result = Position.row.GetHashCode();
                result = (result * 397) ^ Position.col.GetHashCode();
                return result;
            }
            public long ShortestDistanceTo(Galaxy otherGalaxy)
            {
                var xd = Position.col - otherGalaxy.Position.col;
                var yd = Position.row - otherGalaxy.Position.row;

                return Math.Abs(xd) + Math.Abs(yd);
            }
        }
    }
}