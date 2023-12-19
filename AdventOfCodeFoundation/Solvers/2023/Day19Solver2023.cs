using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/19")]
    internal class Day19Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rawSplit = raw.Split("\r\n\r\n");

            var workflows = rawSplit[0].Split("\r\n").Select(x => new Workflow(x)).ToDictionary(x => x.Id);
            var rulesInputString = rawSplit[1].Split("\r\n");
            var ruleInputs = rulesInputString.Select(x => new RuleInput(x)).ToList();
            var sum = 0L;
            foreach (var ruleInput in ruleInputs)
            {
                var currentWorkFlow = workflows.First().Value;
                while (true)
                {
                    var e = currentWorkFlow.EvaluateRule(ruleInput);
                    if (workflows.ContainsKey(e))
                    {
                        currentWorkFlow = workflows[e];
                    }
                    else if (e == "R")
                    {
                        break;
                    }
                    else
                    {
                        //Accepted
                        sum += ruleInput.Score;
                    }

                }
            }
            return sum.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();

            return "";
        }

        class Workflow
        {
            public string Id { get; set; }
            public List<string> Rules { get; set; }
            public string DefaultId { get; set; }
            public string InputString { get; set; }
            public Workflow(string input)
            {
                var s1 = input.Split("{");
                var s2 = s1[1];
                var rulesString = s2.Split(",");
                Rules = rulesString.Take(rulesString.Length - 1).ToList();
                DefaultId = rulesString.Last().Trim('}');
                Id = s1[0];
                InputString = input;
            }
            public string EvaluateRule(RuleInput ruleInput)
            {
                //IF block
                return "A";
            }
            public override string ToString()
            {
                return InputString;
            }
        }
        class RuleInput
        {
            public RuleInput(string input)
            {
                //{x=787,m=2655,a=1222,s=2876}
                var inSplit = input.Split(",");
                x = long.Parse(inSplit[0].Substring(1).Replace("x=", "").Trim());
                m = long.Parse(inSplit[1].Replace("m=", "").Trim());
                a = long.Parse(inSplit[2].Replace("a=", "").Trim());
                s = long.Parse(inSplit[3].Substring(0, inSplit[3].Length - 1).Replace("s=", "").Trim());
            }

            public long x { get; set; }
            public long m { get; set; }
            public long a { get; set; }
            public long s { get; set; }
            public long Score
            {
                get
                {
                    return x + m + a + s;
                }
            }
            public override string ToString()
            {
                return $"x={x}, m={m}, a={a}, s={s}";
            }
        }

    }
}