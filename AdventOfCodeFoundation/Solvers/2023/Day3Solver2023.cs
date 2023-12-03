using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Xml.Linq;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/3")]
    internal class Day3Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var nodes = GetNodes(raw);
            var symbols = nodes.Where(x => x.Type == NodeType.Symbol).Select(x => x).ToList();
            var numbers = nodes.Where(x => x.Type == NodeType.Number).Select(x => x).ToList();
            var adjacentNodes = Convert2DListTo1D<(int, int)>(symbols.Select(x => x.GetAdjacentNodes()).ToList());
            var intersects = numbers.Where(x => x.Coordinates.Intersect(adjacentNodes).ToList().Count >= 1);
            return intersects.Sum(x => x.IntValue).ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var nodes = GetNodes(raw);
            var symbols = nodes.Where(x => x.Type == NodeType.Symbol && x.Value.Equals("*")).Select(x => x).ToList();
            var numbers = nodes.Where(x => x.Type == NodeType.Number).Select(x => x).ToList();
            var intersects = new List<(int, int)>();
            foreach (var symbol in symbols)
            {
                var adNodes = symbol.GetAdjacentNodes();
                var symbolIntersects = numbers.Where(x => x.Coordinates.Intersect(adNodes).ToList().Count >= 1).ToList();
                if (symbolIntersects.Count > 1)
                {
                    intersects.Add((symbolIntersects.First().IntValue, symbolIntersects.Last().IntValue));
                }
            }
            return intersects.Sum(x => x.Item1 * x.Item2).ToString();
        }
        private List<SchemNode> GetNodes(string input)
        {
            var rows = input.Split("\r\n");
            var nodes = new List<SchemNode>();
            for (var i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] == '.') { continue; }
                    if (char.IsSymbol(rows[i][j]) || char.IsPunctuation(rows[i][j]))
                    {
                        var o = new SchemNode(NodeType.Symbol, rows.Length, rows[0].Length) { Value = rows[i][j].ToString() };
                        o.Coordinates.Add((i, j));
                        nodes.Add(o);
                        continue;
                    }
                    var nValue = "";
                    var n = new SchemNode(NodeType.Number, rows.Length, rows[0].Length) { Value = rows[i][j].ToString() };

                    while (char.IsDigit(rows[i][j]))
                    {
                        nValue += rows[i][j];
                        n.Coordinates.Add((i, j));
                        if (j < rows[i].Length - 1 && char.IsDigit(rows[i][j + 1]))
                        {
                            j++;
                        }
                        else { break; }

                    }
                    n.Value = nValue;
                    nodes.Add(n);
                }
            }
            return nodes;
        }
        public static List<T> Convert2DListTo1D<T>(List<List<T>> array2D)
        {
            List<T> lst = new List<T>();
            foreach (List<T> a in array2D)
            {
                lst.AddRange(a);
            }
            return lst.ToList();
        }

        private class SchemNode
        {
            public NodeType Type { get; set; }
            public string Value { get; set; }
            public int IntValue
            {
                get
                {
                    return int.Parse(Value);
                }
            }
            public int MaxX { get; set; }
            public int MaxY { get; set; }
            //Y,X
            public List<(int, int)> Coordinates { get; set; }

            public SchemNode(NodeType type, int maxY, int maxX)
            {
                Coordinates = new List<(int, int)>();
                Type = type;
                MaxX = maxX;
                MaxY = maxY;
            }
            public List<(int, int)> GetAdjacentNodes()

            {
                var adjacentNodes = new List<(int, int)>();
                foreach (var c in Coordinates)
                {
                    //N
                    if (c.Item1 - 1 >= 0)
                    {
                        adjacentNodes.Add((c.Item1 - 1, c.Item2));
                    }
                    //NE
                    if (c.Item1 - 1 >= 0 && c.Item2 + 1 <= MaxX)
                    {
                        adjacentNodes.Add((c.Item1 - 1, c.Item2 + 1));
                    }
                    //E
                    if (c.Item2 + 1 <= MaxX)
                    {
                        adjacentNodes.Add((c.Item1, c.Item2 + 1));
                    }
                    //SE
                    if (c.Item1 + 1 <= MaxY && c.Item2 + 1 <= MaxX)
                    {
                        adjacentNodes.Add((c.Item1 + 1, c.Item2 + 1));
                    }
                    //S
                    if (c.Item1 + 1 <= MaxY)
                    {
                        adjacentNodes.Add((c.Item1 + 1, c.Item2));
                    }
                    //SW
                    if (c.Item1 + 1 <= MaxY && c.Item2 - 1 >= 0)
                    {
                        adjacentNodes.Add((c.Item1 + 1, c.Item2 - 1));
                    }
                    //W
                    if (c.Item2 - 1 >= 0)
                    {
                        adjacentNodes.Add((c.Item1, c.Item2 - 1));
                    }
                    //NW
                    if (c.Item1 - 1 >= 0 && c.Item2 - 1 >= 0)
                    {
                        adjacentNodes.Add((c.Item1 - 1, c.Item2 - 1));
                    }
                }

                return adjacentNodes;
            }
            public override string ToString()
            {
                return Value;
            }
        }
        private enum NodeType
        {
            Symbol,
            Number
        }
    }
}