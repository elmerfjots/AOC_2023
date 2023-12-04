using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/4")]
    internal class Day4Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").Select(x => new ScratchCard(x));

            var intersects = rows.Select(x => x.Numbers.Intersect(x.WinningNumbers)).ToList();
            var sums = new List<int>();
            foreach (var row in intersects.Where(x => x.Any()).Select(x => x.ToList()))
            {
                if (row.Count == 1) { sums.Add(1); continue; }
                var d = 1;
                for (var i = 1; i < row.Count; i++)
                {
                    d = d * 2;
                }
                sums.Add((int)d);
            }
            return sums.Sum().ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();

            return "";
        }

        private class ScratchCard
        {
            public List<int> WinningNumbers { get; set; }
            public List<int> Numbers { get; set; }

            public ScratchCard(string x)
            {
                var s = x.Split(": ").Last().Split(" | ");
                WinningNumbers = s.First().Split(" ").Where(x => string.IsNullOrEmpty(x) == false).Select(n => int.Parse(n)).ToList();
                Numbers = s.Last().Split(" ").Where(x => string.IsNullOrEmpty(x) == false).Select(n => int.Parse(n.Trim())).ToList();

            }
        }
    }
}