using AdventOfCodeFoundation.IO;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/12")]
    internal class Day12Solver2023 : ISolver
    {
        private List<int> groups = new List<int>();
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            long arrangements = 0;
            foreach (var conditionRecord in raw.Split("\r\n").ToList())
            {
                var cRec = conditionRecord.Split(" ").First();
                groups = conditionRecord.Split(" ").Last().Split(",").Select(int.Parse).ToList();
                var memoization = new Dictionary<(int wSt, int cGrp), long>();
                arrangements += FindArrangements(cRec, 0, 0, memoization);
            }
            return arrangements.ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            long arrangements = 0;
            foreach (var conditionRecord in raw.Split("\r\n").ToList())
            {
                var record = conditionRecord.Split(" ").ToArray();
                var unfoldedRecord = record.First() + "?" + record.First() + "?" + record.First() + "?" + record.First() + "?" + record.First();
                groups = conditionRecord.Split(" ").Last().Split(",").Select(int.Parse).ToList();
                var lGroups = groups.ToList();

                for (var i = 0; i < 4; i++)
                {
                    foreach (var group in groups)
                    {
                        lGroups.Add(group);
                    }
                }
                groups = lGroups;
                var memoization = new Dictionary<(int wSt, int cGrp), long>();
                arrangements += FindArrangements(unfoldedRecord, 0, 0, memoization);
            }
            return arrangements.ToString();
        }
        private long FindArrangements(string currentRecordPart, int windowStart, int currentGroupIdx,
            Dictionary<(int wSt, int cGrp), long> memoization)
        {
            long arrangements = 0;
            int wStart = windowStart;
            bool ready = false;
            if (memoization.ContainsKey((windowStart, currentGroupIdx)))
            {
                return memoization[(windowStart, currentGroupIdx)];
            }
            while (!ready && wStart <= currentRecordPart.Length - groups[currentGroupIdx])
            {
                string s = currentRecordPart.Substring(wStart, groups[currentGroupIdx]);
                var wEnd = wStart + groups[currentGroupIdx];
                if (!s.Contains('.'))
                {
                    if (currentGroupIdx == groups.Count - 1)
                    {
                        var sub = currentRecordPart[wEnd..];
                        if (wEnd == currentRecordPart.Length || sub.Contains("#") == false)
                        {
                            arrangements++;
                        }
                    }
                    else
                    {
                        if (wEnd < currentRecordPart.Length && (currentRecordPart[wEnd] == '.' || currentRecordPart[wEnd] == '?'))
                        {
                            arrangements += FindArrangements(currentRecordPart, wEnd + 1, currentGroupIdx + 1, memoization);
                        }
                    }
                }
                if (currentRecordPart[wStart] == '.' || currentRecordPart[wStart] == '?') wStart++;
                else ready = true;
            }
            memoization.Add((windowStart, currentGroupIdx), arrangements);

           return arrangements;
        }
    }
}
