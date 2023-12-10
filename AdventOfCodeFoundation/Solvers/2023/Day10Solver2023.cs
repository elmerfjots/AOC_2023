using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/10")]
    internal class Day10Solver2023 : ISolver
    {
        private static Dictionary<(char, Direction), List<char>> possibleNodes = new Dictionary<(char, Direction), List<char>>()
        {
            {('|',Direction.N), new List<char>{'|','7','F','S'} },
            {('|',Direction.S), new List<char>{'|','L','J','S'} },

            {('-',Direction.W), new List<char>{'-','F','L','S'} },
            {('-',Direction.E), new List<char>{'-','7','J','S'} },

            {('L',Direction.E), new List<char>{'-','7','J','S'}},
            {('L',Direction.N), new List<char>{'|','7','F','S'}},

            {('J',Direction.W), new List<char>{'-','F','L','S'} },
            {('J',Direction.N), new List<char>{'|','F','7','S'} },

            {('7',Direction.W), new List<char>{'-','F','L','S'} },
            {('7',Direction.S), new List<char>{'|','J','L','S'} },

            {('F',Direction.E), new List<char>{ '-', '7', 'J', 'S'} },
            {('F',Direction.S), new List<char>{ '|', 'J', 'L', 'S'} },

            {('.',Direction.N), new List<char>() },
            {('.',Direction.E), new List<char>() },
            {('.',Direction.W), new List<char>() },
            {('.',Direction.S), new List<char>() },
            {('S',Direction.N), new List<char>{ '|','7','F'} },
            {('S',Direction.S), new List<char>{ '|','L','J'} },
            {('S',Direction.W), new List<char>{ '-','L','F'} },
            {('S',Direction.E), new List<char>{ '-','J','7'} },
        };

        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            long steps = CalculateStepsOnMainLoop(rows).steps;
            return (steps / 2).ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            var mainloop = CalculateStepsOnMainLoop(rows);
            long steps = 0;
            //mainloop.map.PrintToFile();
            for (int i = 0; i < mainloop.map.GetLength(0); i++)
            {
                bool isInsideLoop = false;
                char charOnRow = '.';
                for (int j = 0; j < mainloop.map.GetLength(1); j++)
                {
                    char cx = mainloop.map[i, j];
                    if ("|JLF7-".Contains(cx))
                    {
                        switch (cx)
                        {
                            case '|': isInsideLoop = !isInsideLoop; break;
                            case 'F': charOnRow = 'F'; break;
                            case 'L': charOnRow = 'L'; break;
                            case '7': if (charOnRow == 'L') isInsideLoop = !isInsideLoop; break;
                            case 'J': if (charOnRow == 'F') isInsideLoop = !isInsideLoop; break;
                            case '-': break;
                            default: break;
                        }
                    }
                    else if (cx == '.')
                    {
                        if (isInsideLoop) steps++;
                    }
                }
            }
            return steps.ToString();
        }
        static (char[,] map, long steps) CalculateStepsOnMainLoop(string[] map)
        {
            var grid = new char[map.Length, map[0].Length].InitializeMap(map);
            var mainloopMap = new char[map.Length, map[0].Length].InitializeMapWithValue('.');
            var start = FindStartPosition(grid);
            mainloopMap[start.row, start.col] = 'S';
            long steps = 0;
            var currentDirection = Direction.N;
            if (start.col < grid.GetLength(0) - 1 && "-J7".Contains(grid[start.row, start.col + 1]))
            {
                currentDirection = Direction.E;
            }
            else if (start.row > 0 && "|JL".Contains(grid[start.row + 1, start.col]))
            {
                currentDirection = Direction.S;
            }
            else if (start.col > 0 && "-FL".Contains(grid[start.row, start.col - 1]))
            {
                currentDirection = Direction.W;
            }
            var startFound = false;
            var currentRow = start.row;
            var currentCol = start.col;
            while (startFound == false)
            {
                var currentChar = '.';
                if (currentDirection == Direction.S)
                {
                    currentChar = grid[currentRow + 1, currentCol];
                    if (currentChar == 'J') { currentDirection = Direction.W; }
                    else if (currentChar == 'L') { currentDirection = Direction.E; }
                    else if (currentChar == '-') { currentDirection = Direction.UNKNOWN; }
                    currentRow++;
                }
                else if (currentDirection == Direction.E)
                {
                    currentChar = grid[currentRow, currentCol + 1];
                    if (currentChar == 'J') { currentDirection = Direction.N; }
                    else if (currentChar == '7') { currentDirection = Direction.S; }
                    else if (currentChar == '|') { currentDirection = Direction.UNKNOWN; }
                    currentCol++;
                }
                else if (currentDirection == Direction.N)
                {
                    currentChar = grid[currentRow - 1, currentCol];
                    if (currentChar == 'F') { currentDirection = Direction.E; }
                    else if (currentChar == '7') { currentDirection = Direction.W; }
                    else if (currentChar == '-') { currentDirection = Direction.UNKNOWN; }
                    currentRow--;
                }
                else if (currentDirection == Direction.W)
                {
                    currentChar = grid[currentRow, currentCol - 1];
                    if (currentChar == 'F') { currentDirection = Direction.S; }
                    else if (currentChar == 'L') { currentDirection = Direction.N; }
                    else if (currentChar == '|') { currentDirection = Direction.UNKNOWN; }
                    currentCol--;
                }
                if(currentDirection == Direction.UNKNOWN)
                {
                    throw new Exception("Direction cannot be unknown!");
                }
                mainloopMap[currentRow, currentCol] = currentChar;
                startFound = currentChar.Equals('S');
                steps++;
            }
            return (mainloopMap, steps);
        }

        static (int row, int col) FindStartPosition(char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 'S')
                    {
                        return (i, j);
                    }
                }
            }
            throw new InvalidOperationException("Starting position (S) not found on the map.");
        }
        private enum Direction
        {
            N,
            S,
            E,
            W,
            UNKNOWN
        }
    }
}