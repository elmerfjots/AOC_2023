using AdventOfCodeFoundation.IO;
namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2015/12/2")]
    internal class Day2Solver2015 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").Select(x => x.Split("x").Select(x => int.Parse(x)).ToList());
            return rows.Sum(GetWrappingPaperAmount).ToString();
        }
        
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").Select(x => x.Split("x").Select(x => int.Parse(x)).ToList());
            return rows.Sum(GetRibbonAmount).ToString();
        }
        private int GetRibbonAmount(List<int> dimensions)
        {
            (int l, int w, int h) = (dimensions[0], dimensions[1], dimensions[2]);
            var orderedSides = (new int[] { l, w, h }).Order().ToList();
            var r1 = orderedSides[0] + orderedSides[0] + orderedSides[1] + orderedSides[1];
            var r2 = orderedSides[0] * orderedSides[1] * orderedSides[2];
            return r1+r2;
        }
        private int GetWrappingPaperAmount(List<int> dimensions)
        {
            (int l, int w, int h) = (dimensions[0], dimensions[1], dimensions[2]);
            (int s1, int s2, int s3) = ((l * w), (w * h), (h * l));
            var smallestSide = (new int[] { s1, s2, s3 }).Min();
            return (2 * s1) + (2 * s2) + (2 * s3) + smallestSide;
        }
    }
}