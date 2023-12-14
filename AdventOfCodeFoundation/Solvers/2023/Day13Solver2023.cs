using AdventOfCodeFoundation.IO;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/13")]
    internal class Day13Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var maps = GetMaps(raw.Split("\r\n"));
            long sum = 0;
            foreach (var map in maps)
            {
                sum += GetPatternNotesSum(map);
            }


            return sum.ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var maps = GetMaps(raw.Split("\r\n"));
            long sum = 0;
            foreach (var map in maps)
            {
                sum += GetPatternNotesSum(map, true);
            }


            return sum.ToString();
        }
        private long GetPatternNotesSum(List<string> maps, bool part2 = false)
        {
            long sum = 0;

            for (int row = 0; row < maps.Count - 1; row++)
            {
                int smudge = 0;
                var rowOk = true;
                for (var k = 0; k < maps.Count; k++)
                {
                    var up = row - k;
                    var down = row + 1 + k;
                    if (0 <= up && up < down && down < maps.Count)
                    {
                        for (int col = 0; col < maps[0].Length; col++)
                        {
                            if (maps[up][col] != maps[down][col])
                            {
                                rowOk = false;
                                smudge += 1;
                            }
                        }
                    }

                }

                if (part2 && smudge == 1) { sum += 100 * (row + 1); }
                else if (part2 == false && rowOk)
                {
                    sum += 100 * (row + 1);
                }
            }

            for (int col = 0; col < maps[0].Length - 1; col++)
            {
                var colOk = true;
                int smudge = 0;
                for (var k = 0; k < maps[0].Length; k++)
                {
                    var left = col - k;
                    var right = col + 1 + k;
                    if (0 <= left && left < right && right < maps[0].Length)
                    {
                        for (int row = 0; row < maps.Count; row++)
                        {
                            if (maps[row][left] != maps[row][right])
                            {
                                colOk = false;
                                smudge += 1;
                            }
                        }
                    }
                }
                if (part2 && smudge == 1) { sum += 1 + col; }
                if (part2 == false && colOk)
                {
                    sum += 1 + col;
                }
            }
            return sum;
        }

        public List<List<string>> GetMaps(string[] input)
        {
            var maps = new List<List<string>>();
            var currentMap = new List<string>();
            for (var i = 0; i < input.Length; i++)
            {
                var s = input[i];

                if (string.IsNullOrEmpty(s))
                {
                    maps.Add(currentMap);
                    currentMap = new List<string>();
                    continue;
                }
                else if (i == input.Length - 1)
                {
                    currentMap.Add(s);
                    maps.Add(currentMap);
                    continue;
                }
                currentMap.Add(s);
            }
            return maps;
        }
    }
}