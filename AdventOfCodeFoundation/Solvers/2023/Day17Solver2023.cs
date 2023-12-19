using AdventOfCodeFoundation.Common;
using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
using Microsoft.VisualBasic;
using System.ComponentModel;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/17")]
    internal class Day17Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            var map = new int[rawMap.Length, rawMap[0].Length].InitializeMap(rawMap);
            var queue = new PriorityQueue<Path, int>();
            var visited = new HashSet<string>();

            queue.Enqueue(new Path(new(0, 0), Direction.East, 0), 0);

            var totalHeat = 0;

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();

                if (path.Position.Row == map.GetLength(0) - 1 && path.Position.Col == map.GetLength(1) - 1)
                {
                    totalHeat = path.Heat;
                    break;
                }

                if (path.Distance < 3)
                {
                    TryMove(path, path.Direction, ref visited, ref queue, ref map);
                }

                TryMove(path, path.Direction.TurnLeft(), ref visited, ref queue, ref map);
                TryMove(path, path.Direction.TurnRight(), ref visited, ref queue, ref map);
            }

            return totalHeat.ToString();
        }


        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            var map = new int[rawMap.Length, rawMap[0].Length].InitializeMap(rawMap);
            var queue = new PriorityQueue<Path, int>();
            var visited = new HashSet<string>();

            queue.Enqueue(new Path(new(0, 0), Direction.South, 0), 0);

            var totalHeat = 0;

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();

                if (path.Position.Row == map.GetLength(0) - 1 && path.Position.Col == map.GetLength(1) - 1)
                {
                    totalHeat = path.Heat;
                    break;
                }

                if (path.Distance < 10)
                {
                    TryMove(path, path.Direction, ref visited, ref queue, ref map);
                }
                if (path.Distance >= 4)
                {
                    TryMove(path, path.Direction.TurnLeft(), ref visited, ref queue, ref map);
                    TryMove(path, path.Direction.TurnRight(), ref visited, ref queue, ref map);
                }
            }

            return totalHeat.ToString();
        }
        void TryMove(Path path, Direction direction, ref HashSet<string> visited, ref PriorityQueue<Path, int> queue, ref int[,] map)
        {
            var node = new Path(path.Position.Move(direction), direction, direction == path.Direction ? path.Distance + 1 : 1);

            if (node.Position.Row < 0 || node.Position.Row >= map.GetLength(0) ||
                node.Position.Col < 0 || node.Position.Col >= map.GetLength(1))
            {
                return;
            }

            var key = $"{node.Position.Row},{node.Position.Col},{node.Direction.Row},{node.Direction.Col},{node.Distance}";
            if (visited.Contains(key))
            {
                return;
            }

            visited.Add(key);

            node.Heat = path.Heat + map[node.Position.Row, node.Position.Col];
            queue.Enqueue(node, node.Heat);
        }
        internal class Path(Position position, Direction direction, int distance)
        {
            public readonly Position Position = position;
            public readonly Direction Direction = direction;
            public readonly int Distance = distance;
            public int Heat { get; set; }
        }

        internal class Direction(int row, int col)
        {
            public readonly int Row = row;
            public readonly int Col = col;

            public Direction TurnLeft()
            {
                return new Direction(-Col, Row);
            }

            public Direction TurnRight()
            {
                return new Direction(Col, -Row);
            }

            public static Direction North = new(-1, 0);
            public static Direction South = new(1, 0);
            public static Direction West = new(0, -1);
            public static Direction East = new(0, 1);
        }

        internal class Position(int row, int col)
        {
            public readonly int Row = row;
            public readonly int Col = col;

            public Position Move(Direction dir)
            {
                return new Position(Row + dir.Row, Col + dir.Col);
            }
        }
    }
}