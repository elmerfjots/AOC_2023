using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/21")]
    internal class Day21Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var mapSPlit = raw.Split("\r\n");
            //var map = new string[mapSPlit.Length, mapSPlit[0].Length].InitializeMap(mapSPlit);
            int numSteps = 64; // Example steps
            int result = CountReachablePositions(mapSPlit, numSteps);

            return result.ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var mapSPlit = raw.Split("\r\n");
            //var map = new string[mapSPlit.Length, mapSPlit[0].Length].InitializeMap(mapSPlit);


            int numSteps = 10; // Example steps
            int result = CountReachablePositions(mapSPlit, numSteps, true);

            return result.ToString();
        }
        private static string[] ExpandMap(string[] input)
        {
            int originalRows = input.Length;
            int originalCols = input[0].Length;
            // Create a new, larger 2D char array
            char[,] expandedMap = new char[originalRows * 2, originalCols * 2];

            // Copy the original map into each quadrant of the new array
            for (int row = 0; row < originalRows * 2; row++)
            {
                for (int col = 0; col < originalCols * 2; col++)
                {
                    expandedMap[row, col] = input[row % originalRows][col % originalCols];
                }
            }

            // Convert the expanded map back into a string array
            string[] expandedMapStringArray = new string[originalRows * 2];
            for (int row = 0; row < originalRows * 2; row++)
            {
                char[] rowChars = new char[originalCols * 2];
                for (int col = 0; col < originalCols * 2; col++)
                {
                    rowChars[col] = expandedMap[row, col];
                }
                expandedMapStringArray[row] = new string(rowChars);
            }

            return expandedMapStringArray;
        }


        public static int CountReachablePositions(string[] input, int numSteps, bool part2 = false)
        {
            var visited = new HashSet<(int, int)>();
            var q = new List<(int, int)>();
            int m = input.Length;
            int n = input[0].Length;
            var dirs = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };

            // Find the start position
            for (int r = 0; r < m; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    if (input[r][c] == 'S')
                    {
                        q.Add((r, c));
                    }
                }
            }

            // Perform the steps
            for (int i = 0; i < numSteps; i++)
            {
                var toVisit = new HashSet<(int, int)>();
                foreach (var (cr, cc) in q)
                {
                    visited.Add((cr, cc));
                    foreach (var dir in dirs)
                    {
                        int nr = cr + dir.Item1;
                        int nc = cc + dir.Item2;
                        if (part2 == false && (nr < 0 || nr >= m || nc < 0 || nc >= n)) continue;
                        else if ((nr < 0 || nr >= m || nc < 0 || nc >= n))
                        {
                            Console.WriteLine("Expand...");
                            input = ExpandMap(input);

                        }
                        if ((input[nr][nc] == '.' || input[nr][nc] == 'S') && !toVisit.Contains((nr, nc)))
                        {
                            toVisit.Add((nr, nc));
                        }
                    }
                }
                q.Clear();
                q.AddRange(toVisit);
            }

            return q.Count;
        }

    }

}