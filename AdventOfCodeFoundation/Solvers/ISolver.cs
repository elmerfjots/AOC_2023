using AdventOfCodeFoundation.IO;
using System.Diagnostics;

namespace AdventOfCodeFoundation.Solvers
{
    internal interface ISolver
    {
        public Task<string> SolvePartOne(Input input);
        public Task<string> SolvePartTwo(Input input);

        public async Task Run(DateOnly challengeDate)
        {
            Output.Line($"Solving {challengeDate} with {GetType().Name}...\n");

            var input = new Input(challengeDate);
            await SolvePart(1, input);
            await SolvePart(2, input);
        }

        private async Task SolvePart(int part, Input input)
        {
            var stopwatch = new Stopwatch();
            Output.Line($"Solving part {part}...");            
            stopwatch.Start();
            var res = await (part == 1 ? SolvePartOne(input) : SolvePartTwo(input));
            stopwatch.Stop();
            Output.Line($"Solved part {part} in {stopwatch.ElapsedMilliseconds}ms");
            Output.Line($"Result: {res}\n");
        }
    }
}
