using AdventOfCodeFoundation.Common;
using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/16")]
    internal class Day16Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            var map = new char[rawMap.Length, rawMap[0].Length].InitializeMap(rawMap);
            return CalculateEnergizedTiles((0, 0, Direction.East), map).ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rawMap = raw.Split("\r\n");
            var map = new char[rawMap.Length, rawMap[0].Length].InitializeMap(rawMap);
            var energizedTiles = 0L;
            for (var row = 0; row < map.GetLength(0); row++)
            {
                energizedTiles = long.Max(energizedTiles, CalculateEnergizedTiles((row, 0, Direction.East), map));
                energizedTiles = long.Max(energizedTiles, CalculateEnergizedTiles((row, map.GetLength(1) - 1, Direction.West), map));
            }
            for (var col = 0; col < map.GetLength(1); col++)
            {
                energizedTiles = long.Max(energizedTiles, CalculateEnergizedTiles((0, col, Direction.South), map));
                energizedTiles = long.Max(energizedTiles, CalculateEnergizedTiles((map.GetLength(0) - 1, col, Direction.North), map));
            }
            return energizedTiles.ToString();
        }
        

        private long CalculateEnergizedTiles((int row, int col, Direction dir) startPosition, char[,] map)
        {
            var beams = new Queue<LightBeam>();
            beams.Enqueue(new LightBeam(startPosition.dir, (startPosition.row, startPosition.col)));
            var lightMap = new char[map.GetLength(0), map.GetLength(1)].InitializeMap(map);
            var energized = 0;
            var seen = new HashSet<(Direction, int row, int col)>();
            while (beams.Any())
            {
                var beam = beams.Dequeue();
                if (seen.Contains((beam.Direction, beam.Position.row, beam.Position.col))) { continue; }
                if (beam.Position.col > map.GetLength(1) - 1) { continue; }
                if (beam.Position.col < 0) { continue; }
                if (beam.Position.row > map.GetLength(0) - 1) { continue; }
                if (beam.Position.row < 0) { continue; }
                seen.Add((beam.Direction, beam.Position.row, beam.Position.col));
                var newDirections = GetDirections(beam.Direction, map[beam.Position.row, beam.Position.col]);
                if (newDirections.Count > 1)
                {
                    foreach (var direction in newDirections.Skip(1))
                    {
                        var newPosition = GetRowColFromDirection(direction, beam.Position);
                        var newBeam = (new LightBeam(direction, newPosition));
                        beams.Enqueue(newBeam);
                    }
                }
                var nBeam = new LightBeam(beam);
                nBeam.Direction = newDirections.First();
                nBeam.Position = GetRowColFromDirection(newDirections.First(), beam.Position);
                beams.Enqueue(nBeam);
                if (lightMap[beam.Position.row, beam.Position.col] != '#')
                {
                    lightMap[beam.Position.row, beam.Position.col] = '#';
                    energized++;
                }
            }
            return energized;
        }
        private (int row, int col) GetRowColFromDirection(Direction dir, (int row, int col) pos)
        {

            if (dir == Direction.North)
            {
                return (pos.row - 1, pos.col);
            }
            if (dir == Direction.East)
            {
                return (pos.row, pos.col + 1);
            }
            if (dir == Direction.West)
            {
                return (pos.row, pos.col - 1);
            }
            return (pos.row + 1, pos.col);
        }
        private List<Direction> GetDirections(Direction dir, char mirror)
        {
            if (dir == Direction.East && mirror == '/')
            {
                return new List<Direction>() { Direction.North };
            }
            if (dir == Direction.East && mirror == '\\')
            {
                return new List<Direction>() { Direction.South };
            }
            if (dir == Direction.East && mirror == '|')
            {
                return new List<Direction>() { Direction.South, Direction.North };
            }

            if (dir == Direction.West && mirror == '/')
            {
                return new List<Direction>() { Direction.South };
            }
            if (dir == Direction.West && mirror == '\\')
            {
                return new List<Direction>() { Direction.North };
            }
            if (dir == Direction.West && mirror == '|')
            {
                return new List<Direction>() { Direction.South, Direction.North };
            }

            if (dir == Direction.North && mirror == '/')
            {
                return new List<Direction>() { Direction.East };
            }
            if (dir == Direction.North && mirror == '\\')
            {
                return new List<Direction>() { Direction.West };
            }
            if (dir == Direction.North && mirror == '-')
            {
                return new List<Direction>() { Direction.West, Direction.East };
            }

            if (dir == Direction.South && mirror == '/')
            {
                return new List<Direction>() { Direction.West };
            }
            if (dir == Direction.South && mirror == '\\')
            {
                return new List<Direction>() { Direction.East };
            }
            if (dir == Direction.South && mirror == '-')
            {
                return new List<Direction>() { Direction.West, Direction.East };
            }

            return new List<Direction>() { dir };

        }
        class LightBeam
        {
            public Direction Direction { get; set; }
            public (int row, int col) Position { get; set; }
            public LightBeam(Direction dir, (int row, int col) pos)
            {
                Position = pos;
                Direction = dir;
            }
            public LightBeam(LightBeam b)
            {
                Direction = b.Direction;
                Position = b.Position;
            }

        }
    }
}