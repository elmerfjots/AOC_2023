using AdventOfCodeFoundation.IO;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/1")]
    internal class Day1Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var cDoc = await input.GetRawInput();
            var inputArr = cDoc.Split("\r\n");
            var resultArray = ParseDigits(inputArr);
            return resultArray.Sum().ToString();
        }
        
        public async Task<string> SolvePartTwo(Input input)
        {
            var cDoc = await input.GetRawInput();
            var inputArr = ReplaceStrings(cDoc).Split("\r\n");
            var resultArray = ParseDigits(inputArr);
            return resultArray.Sum().ToString();
        }
        private List<int> ParseDigits(string[] input)
        {
            var digitArr = new List<int>();
            foreach (var s in input)
            {
                var digits = s.Where(c => char.IsDigit(c)).Select(x => x.ToString()).ToList();

                var t = digits.FirstOrDefault();
                var t2 = digits.LastOrDefault();
                if (t == null || t2 == null)
                {
                    continue;
                }
                digitArr.Add(int.Parse(t + t2));
            }
            return digitArr;
        }
        string ReplaceStrings(string input)
        {
            var cDoc = input.Replace("one", "one1one");
            cDoc = cDoc.Replace("two", "two2two");
            cDoc = cDoc.Replace("three", "three3three");
            cDoc = cDoc.Replace("four", "four4four");
            cDoc = cDoc.Replace("five", "five5five");
            cDoc = cDoc.Replace("six", "six6six");
            cDoc = cDoc.Replace("seven", "seven7seven");
            cDoc = cDoc.Replace("eight", "eight8eight");
            cDoc = cDoc.Replace("nine", "nine9nine");
            return cDoc;
        }
    }
}
