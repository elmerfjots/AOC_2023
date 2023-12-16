using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/15")]
    internal class Day15Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var strings = raw.Split(",");
            var sum = strings.Sum(GetHashOfString);
            return sum.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();

            return "";
        }
        private long GetHashOfString(string s)
        {
            /*  Determine the ASCII code for the current character of the string.
                    Increase the current value by the ASCII code you just determined.
                    Set the current value to itself multiplied by 17.
                    Set the current value to the remainder of dividing itself by 256.
                */
            long currentValue = 0;
            foreach (var c in s)
            {
                int asciiCode = (int)c;
                currentValue += asciiCode;
                currentValue *= 17;
                currentValue %= 256;
            }
            return currentValue;
        }
    }
}