using AdventOfCodeFoundation.IO;

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
            var lenses = raw.Split(",").Select(x => new Lens(x));
            var boxes = Enumerable.Range(0, 256).Select(x => new Dictionary<string, long>()).ToList();
            foreach (var lens in lenses) { CalculateBoxesForLens(ref boxes, lens); }
            return CalculateSumOfFocusingPower(ref boxes).ToString();
        }

        private long CalculateSumOfFocusingPower(ref List<Dictionary<string, long>> boxes)
        {
            var sum = 0L;
            for (var boxNumber = 0; boxNumber < boxes.Count; boxNumber++)
            {
                var box = boxes[boxNumber];

                for (var slot = 0; slot < box.Count; slot++)
                {
                    var e = box.ElementAt(slot);
                    var focusingPower = (boxNumber + 1) * (slot + 1) * e.Value;
                    sum += focusingPower;
                }

            }
            return sum;
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
        private void CalculateBoxesForLens(ref List<Dictionary<string, long>> boxes, Lens lens)
        {
            var relevantBoxNumber = (int)GetHashOfString(lens.Label);
            if (lens.Operation.Equals("="))
            {
                if (boxes[relevantBoxNumber].ContainsKey(lens.Label))
                {
                    boxes[relevantBoxNumber][lens.Label] = lens.FocalLength;
                }
                else
                {
                    boxes[relevantBoxNumber] = boxes[relevantBoxNumber]
                        .Append(new KeyValuePair<string, long>(lens.Label, lens.FocalLength))
                        .ToDictionary(x => x.Key, x => x.Value);
                }
            }
            if (lens.Operation.Equals("-"))
            {
                if (boxes[relevantBoxNumber].ContainsKey(lens.Label))
                {
                    boxes[relevantBoxNumber].Remove(lens.Label);
                }
            }
        }
        class Lens
        {
            public string Label { get; set; }
            public long FocalLength { get; set; }
            public string Operation { get; set; }
            public Lens(string input)
            {
                string[] ss = new string[2];
                if (input.Contains('='))
                {
                    ss = input.Split('=');
                    FocalLength = long.Parse(ss[1]);
                    Operation = "=";
                }
                else if (input.Contains('-'))
                {
                    ss = input.Split('-');
                    Operation = "-";
                }
                Label = ss.First();
            }
        }
    }
}