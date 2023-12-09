using AdventOfCodeFoundation.IO;
namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/9")]
    internal class Day9Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            var histories = rows.Select(x => x.Split(" ").ToList().Select(x => int.Parse(x.ToString())).ToList()).ToList();
            return histories.Sum(ExtrapolateForward).ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            var histories = rows.Select(x => x.Split(" ").ToList().Select(x => int.Parse(x.ToString())).ToList()).ToList();
            return histories.Sum(ExtrapolateBackward).ToString(); 
        }
        private int ExtrapolateForward(List<int> sequence)
        {
            var info = CreateInfo(sequence);
            foreach (var s in info)
            {
                s.Add(s.Last());
            }
            return info.Sum(x => x.Last());
        }
        private int ExtrapolateBackward(List<int> sequence)
        {
            var info = CreateInfo(sequence);
            var curDiff = 0;
            for (var i = info.Count - 1; i >= 0; i--)
            {
                if (info[i].All(x => x.Equals(0))) { info[i].Insert(0, 0); curDiff = 0; continue; }
                curDiff = info[i].First() - curDiff;
                info[i].Insert(0, curDiff);
            }
            return info.First().First();
        }
        private List<int> Differences(List<int> sequence)
        {
            List<int> newSequence = new List<int>();
            for (int i = 1; i < sequence.Count; i++)
            {
                int newElement = sequence[i] - sequence[i - 1];
                newSequence.Add(newElement);
            }
            return newSequence;
        }
        private List<List<int>> CreateInfo(List<int> sequence)
        {
            var info = new List<List<int>>() { sequence };
            while (!sequence.All(x => x.Equals(0)))
            {
                sequence = Differences(sequence);
                info.Add(sequence);
            }
            return info;
        }
    }
}