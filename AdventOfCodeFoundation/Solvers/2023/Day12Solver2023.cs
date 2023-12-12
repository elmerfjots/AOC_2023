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
                var memoization = new Dictionary<(int, int, int), long>();
                arrangements += FindArrangements(cRec, groups, 0, 0, 0, memoization);
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
                var memoization = new Dictionary<(int, int, int), long>();
                arrangements += FindArrangements(unfoldedRecord, groups, 0, 0, 0, memoization);
            }
            return arrangements.ToString();
        }
        private long FindArrangements(string record, List<int> groups,
            int currentPositionInRecord,
            int currentGroupPosition,
            int lengthOfCurrentGroup,
            Dictionary<(int, int, int), long> memoization)
        {
            //Terminate - Vi er ved ved enden i rekord
            if (memoization.ContainsKey((currentPositionInRecord, currentGroupPosition, lengthOfCurrentGroup)))
            {
                return memoization[(currentPositionInRecord, currentGroupPosition, lengthOfCurrentGroup)];
            }
            if (currentPositionInRecord == record.Length)
            {
                if (currentGroupPosition == groups.Count && lengthOfCurrentGroup == 0)
                {
                    return 1L;
                }
                else if (currentGroupPosition == groups.Count - 1 && groups[currentGroupPosition] == lengthOfCurrentGroup)
                {
                    return 1L;
                }

                else
                {
                    return 0;
                }
            }
            long arrangements = 0;
            //Vi kan ikke terminere endnu!
            //Vi kan fylde med . eller #
            foreach (var c in ".#")
            {
                //Hvis det er ? er det lige meget hvad vi fylder med. Ellers er det én af dem.
                if (record[currentPositionInRecord] == c || record[currentPositionInRecord] == '?')
                {
                    //Hvis vi lige har haft en . , er vi ikke i gang med noget, så vi går videre i rekord
                    if (c == '.' && lengthOfCurrentGroup == 0)
                    {
                        arrangements += FindArrangements(record, groups, currentPositionInRecord + 1, currentGroupPosition, 0, memoization);
                    }
                    //Vi afslutter gruppen. Derfor skal vi gå videre i både rekord og gruppe
                    else if (c == '.' && lengthOfCurrentGroup > 0 && currentGroupPosition < groups.Count && groups[currentGroupPosition] == lengthOfCurrentGroup)
                    {
                        arrangements += FindArrangements(record, groups, currentPositionInRecord + 1, currentGroupPosition + 1, 0, memoization);
                    }
                    //Hvis vi fylder med # så øger vi længden af den gruppe af ødelagte kilder.
                    else if (c == '#')
                    {
                        arrangements += FindArrangements(record, groups, currentPositionInRecord + 1, currentGroupPosition, lengthOfCurrentGroup + 1, memoization);
                    }
                }
            }
            memoization.Add((currentPositionInRecord, currentGroupPosition, lengthOfCurrentGroup), arrangements);
            return arrangements;
        }
    }
}
