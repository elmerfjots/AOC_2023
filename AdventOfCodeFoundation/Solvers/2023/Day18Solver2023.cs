using AdventOfCodeFoundation.Extensions;
using AdventOfCodeFoundation.IO;
using System.Data;
using System.Text;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/18")]
    internal class Day18Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            var plan = new Digplan(rows);
            var area = plan.CalculateArea();
            return area.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");

            var plan = new Digplan(rows, true);
            var area = plan.CalculateArea();
            return area.ToString();
        }
        class Digplan
        {
            public List<Instruction> Instructions { get; set; }
            public List<DigEdge> Edges { get; set; }

            public HashSet<(long row, long col)> Points { get; set; }
            public string[,] Map { get; set; }
            public Digplan(string[] rows, bool useHex = false)
            {
                Instructions = rows.Select(x => new Instruction(x, useHex)).ToList();
                Edges = new List<DigEdge>();
                Points = new HashSet<(long row, long col)>();
                DigTrench();
            }
            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var instruction in Instructions)
                {
                    sb.AppendLine(instruction.ToString());
                }
                return sb.ToString();
            }

            internal void DigTrench()
            {
                var currentPos = (0L, 0L);
                Points.Add(currentPos);
                foreach (var instruction in Instructions)
                {

                    (long row, long col) endPoint = currentPos;
                    if (instruction.DigDirection == Direction.North)
                    {
                        endPoint = (endPoint.row - instruction.DigLength, endPoint.col);
                    }
                    if (instruction.DigDirection == Direction.East)
                    {
                        endPoint = (endPoint.row, endPoint.col + instruction.DigLength);
                    }
                    if (instruction.DigDirection == Direction.South)
                    {
                        endPoint = (endPoint.row + instruction.DigLength, endPoint.col);
                    }
                    if (instruction.DigDirection == Direction.West)
                    {
                        endPoint = (endPoint.row, endPoint.col - instruction.DigLength);
                    }
                    var edge = new DigEdge() { StartPoint = currentPos, EndPoint = endPoint };
                    currentPos = endPoint;
                    Edges.Add(edge);
                    Points.Add(currentPos);
                    Points.Add(endPoint);

                }
            }

            internal long CalculateArea()
            {
                long trenchArea = Edges.Sum(x => x.Length);
                long interiorArea = ShoelaceArea(Points);

                return interiorArea + (trenchArea / 2) + 1;
            }
            static long ShoelaceArea(HashSet<(long row, long col)> points)
            {
                long area = 0;
                for (int i = 0; i < points.Count - 1; i++)
                {
                    area += points.ElementAt(i).col * points.ElementAt(i + 1).row - points.ElementAt(i + 1).col * points.ElementAt(i).row;
                }
                return Math.Abs(area + points.Last().col * points.ElementAt(0).row - points.ElementAt(0).col * points.Last().row) / 2;
            }
        }
        class DigEdge
        {
            public (long row, long col) StartPoint { get; set; }
            public (long row, long col) EndPoint { get; set; }
            public long Length
            {
                get
                {
                    return Math.Abs(StartPoint.col - EndPoint.col) + Math.Abs(StartPoint.row - EndPoint.row);
                }
            }
            public override string ToString()
            {
                return $"S({StartPoint.row},{StartPoint.col}) E({EndPoint.row},{EndPoint.col})";
            }
        }
        class Instruction
        {
            public Instruction(string x, bool useHex)
            {
                var s = x.Split(" ");
                var dirString = s[0];
                DigDirection = dirString == "R" ? Direction.East :
                    dirString == "L" ? Direction.West :
                    dirString == "U" ? Direction.North :
                    dirString == "D" ? Direction.South :
                    Direction.Unknown;
                DigLength = long.Parse(s[1]);
                ColorHash = s[2].Trim('(').Trim(')').Trim('#');
                if (useHex)
                {
                    DigLength = long.Parse(ColorHash.Substring(0, 5), System.Globalization.NumberStyles.HexNumber);
                    var directionInt = int.Parse(ColorHash.Last().ToString());
                    DigDirection = directionInt == 0 ? Direction.East :
                        directionInt == 1 ? Direction.South :
                        directionInt == 2 ? Direction.West :
                        directionInt == 3 ? Direction.North :
                        Direction.Unknown;
                }

            }
            public Direction DigDirection { get; set; }
            public long DigLength { get; set; }

            public string ColorHash { get; set; }
            public override string ToString()
            {
                return $"{DigDirection}     {DigLength}     {ColorHash}";
            }
        }
    }
}