using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/14")]
    internal class Day14Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            char[,] map = new char[rawMap.Length, rawMap[0].Length];
            map.InitializeMap(rawMap);
            //PrintMap(map);
            for (var r = 0; r < map.GetLength(0); r++)
            {
                for (var c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 'O')
                    {
                        map = MoveRock(map, r, c, Direction.North);
                        //PrintMap(map);
                    }
                }
            }
            var sum = 0;
            for (var r = 0; r < map.GetLength(0); r++)
            {
                var cnt = Enumerable.Range(0, map.GetLength(1))
                .Select(x => map[r, x])
                .ToList().Count(x => x.Equals('O'));
                sum += cnt * ((map.GetLength(0)) - r);
            }
            //PrintMap(map);
            return sum.ToString();
        }



        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            char[,] map = new char[rawMap.Length, rawMap[0].Length];
            map.InitializeMap(rawMap);
            var cycles = 5000;
            Dictionary<long, long> mapCache = new();


            //In order to get these values, I ran the cycles 5000 times, and then looked for pattern.
            var repeatsAtCycle = 159;
            var firstInRepeatCycle = 94;
            var cyclePatternLength = 65;


            var targetIteration = firstInRepeatCycle + ((1000000000 - repeatsAtCycle) % cyclePatternLength);


            for (var cycle = 0; cycle < 200; cycle++)
            {
                foreach (var dir in new Direction[] { Direction.North, Direction.West, Direction.South, Direction.East })
                {
                    if (dir == Direction.North || dir == Direction.West)
                    {
                        for (var r = 0; r < map.GetLength(0); r++)
                        {
                            for (var c = 0; c < map.GetLength(1); c++)
                            {
                                if (map[r, c] == 'O')
                                {
                                    map = MoveRock(map, r, c, dir);
                                }
                            }
                        }
                    }
                    if (dir == Direction.South || dir == Direction.East)
                    {
                        for (var r = map.GetLength(0) - 1; r >= 0; r--)
                        {
                            for (var c = map.GetLength(1) - 1; c >= 0; c--)
                            {
                                if (map[r, c] == 'O')
                                {
                                    map = MoveRock(map, r, c, dir);
                                }
                            }

                        }
                    }
                }
                var mapHash = map.ComputeHash();
                if (mapCache.TryGetValue(mapHash, out long cy) == false)
                {
                    mapCache.Add(mapHash, cycle);
                    Console.WriteLine($"New map in cycle: {cycle}");
                }
                else
                {
                    Console.WriteLine($"Cycle {cycle} is the same as {cy}");
                }

            }

            var sum = 0;
            for (var r = 0; r < map.GetLength(0); r++)
            {
                var cnt = Enumerable.Range(0, map.GetLength(1))
                .Select(x => map[r, x])
                .ToList().Count(x => x.Equals('O'));
                sum += cnt * ((map.GetLength(0)) - r);
            }
            return sum.ToString();
        }
        private char[,] MoveRock(char[,] map, int r, int c, Direction dir)
        {
            if (dir == Direction.North && r == 0)
            {
                return map;
            }
            if (dir == Direction.West && c == 0)
            {
                return map;
            }
            if (dir == Direction.South && r == map.GetLength(0) - 1)
            {
                return map;
            }
            if (dir == Direction.East && c == map.GetLength(1) - 1)
            {
                return map;
            }

            if (dir == Direction.North)
            {
                var value = map[r - 1, c];
                if ("#O".Contains(value) == false)
                {
                    map[r, c] = '.';
                    map[r - 1, c] = 'O';
                    return MoveRock(map, r - 1, c, dir);
                }

            }
            if (dir == Direction.West)
            {
                var value = map[r, c - 1];
                if ("#O".Contains(value) == false)
                {
                    map[r, c] = '.';
                    map[r, c - 1] = 'O';
                    return MoveRock(map, r, c - 1, dir);
                }

            }
            if (dir == Direction.South)
            {
                var value = map[r + 1, c];
                if ("#O".Contains(value) == false)
                {
                    map[r, c] = '.';
                    map[r + 1, c] = 'O';
                    return MoveRock(map, r + 1, c, dir);
                }

            }
            if (dir == Direction.East)
            {
                var value = map[r, c + 1];
                if ("#O".Contains(value) == false)
                {
                    map[r, c] = '.';
                    map[r, c + 1] = 'O';
                    return MoveRock(map, r, c + 1, dir);
                }

            }
            return map;
        }
        public void PrintMap(char[,] map)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var k = 0; k < map.GetLength(1); k++)
                {
                    sb.Append(map[i, k]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

    }
}