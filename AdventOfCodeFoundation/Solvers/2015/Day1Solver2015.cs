using AdventOfCodeFoundation.IO;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2015/12/1")]
    internal class Day1Solver2015 : ISolver
    {
        Dictionary<char, int> instructions = new Dictionary<char, int>() { { '(', 1 }, { ')', -1 } };
        public async Task<string> SolvePartOne(Input input)
        {
            var rawInput = await input.GetRawInput();
            var s = 0;
            foreach(var i in rawInput)
            {
                s += instructions[i];
            }
            return s.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var rawInput = await input.GetRawInput();
            var s = 0;
            var c = 0;
            foreach (var i in rawInput)
            {
                s += instructions[i];
                c++;
                if (s == -1) { break; }
            }
            return c.ToString();
        }
    }
}
