using AdventOfCodeFoundation.IO;

namespace AdventOfCodeFoundation.Solvers._2015
{
    [Solves("2015/12/1")]
    internal class Day1Solver2015 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            return (await input.GetRawInput()).Aggregate(0, (s, c) => s + ParseChar(c)).ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var instructions = await input.GetRawInput();
            var res = FindCellar(instructions);

            return res.HasValue ? $"{res.Value + 1}" : "No solution found.";
        }

        private static int? FindCellar(string instructions)
        {
            var floor = 0;
            var insts = instructions.TakeWhile(c => (floor += ParseChar(c)) >= 0).ToList();            
            return floor < 0 ? insts.Count : null;
        }

        private static int ParseChar(char c) => c == '(' ? 1 : -1;
    }
}
