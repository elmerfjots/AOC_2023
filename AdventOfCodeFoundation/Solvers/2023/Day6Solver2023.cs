using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Timers;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/6")]
    internal class Day6Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").ToList();
            var times = rows.First().Split(" ").Where(c => c.Any(cc => char.IsDigit(cc))).ToList();
            var distances = rows.Last().Split(" ").Where(c => c.Any(cc => char.IsDigit(cc))).ToList();
            long sum = 1;
            for (var i = 0; i < times.Count; i++)
            {
                var time = times[i];
                var distance = distances[i];
                sum *= CalculateWaysToBeatRecord(long.Parse(time), long.Parse(distance));
            }
            return sum.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").ToList();
            var time = string.Join("", rows.First().Split(" ").Where(c => c.Any(cc => char.IsDigit(cc))));
            var distance = string.Join("", rows.Last().Split(" ").Where(c => c.Any(cc => char.IsDigit(cc))).ToList());
            long sum = CalculateWaysToBeatRecord(long.Parse(time), long.Parse(distance));
            return sum.ToString();
        }
        static long CalculateWaysToBeatRecord(long raceDuration, long recordDistance)
        {
            int waysToBeatRecord = 0;

            for (int holdTime = 1; holdTime < raceDuration; holdTime++)
            {
                long remainingTime = raceDuration - holdTime;
                //Speed * remaining time
                long totalDistance = holdTime * remainingTime;

                if (totalDistance > recordDistance)
                {
                    waysToBeatRecord++;
                }
            }
            return waysToBeatRecord;
        }
    }
}