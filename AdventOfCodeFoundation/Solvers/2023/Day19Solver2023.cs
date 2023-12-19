using AdventOfCodeFoundation.IO;

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
            var sum = ruleInputs.Where(x => RuleInputAccepted(ref workflows, x)).Sum(x => x.Score);
            return sum.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rawSplit = raw.Split("\r\n\r\n");
            var workflows = rawSplit[0].Split("\r\n").Select(x => new Workflow(x)).ToDictionary(x => x.Id);
            var rulesInputString = rawSplit[1].Split("\r\n");
            var ruleQueue = new Queue<(string key, RuleInput rIn)>();
            ruleQueue.Enqueue(("in", new RuleInput(1, 4000)));
            long sum = 0;
            while (ruleQueue.Any())
            {
                (string key, RuleInput rIn) = ruleQueue.Dequeue();
                if (rIn.LowHighs.xl > rIn.LowHighs.xh ||
                    rIn.LowHighs.ml > rIn.LowHighs.mh ||
                    rIn.LowHighs.al > rIn.LowHighs.ah ||
                    rIn.LowHighs.sl > rIn.LowHighs.sh)
                { continue; }
                if (key == "A")
                {
                    sum += (rIn.LowHighs.xh - rIn.LowHighs.xl + 1) *
                        (rIn.LowHighs.mh - rIn.LowHighs.ml + 1) *
                        (rIn.LowHighs.ah - rIn.LowHighs.al + 1) *
                        (rIn.LowHighs.sh - rIn.LowHighs.sl + 1);
                    continue;
                }
                else if (key == "R") { continue; }
                else
                {
                    var workflow = workflows[key];
                    foreach (var rule in workflow.Rules)
                    {
                        var newRanges = GetNewRanges(rule.lh, rule.op, rule.rh, rIn.LowHighs.xl, rIn.LowHighs.xh, rIn.LowHighs.ml, rIn.LowHighs.mh, rIn.LowHighs.al, rIn.LowHighs.ah, rIn.LowHighs.sl, rIn.LowHighs.sh);
                        var rInNew = new RuleInput(newRanges);
                        ruleQueue.Enqueue((rule.trueKey, rInNew));

                        //Negation of operators
                        var newOp = rule.op == ">" ? "<=" : ">=";

                        var nn = GetNewRanges(rule.lh, newOp, rule.rh, rIn.LowHighs.xl, rIn.LowHighs.xh, rIn.LowHighs.ml, rIn.LowHighs.mh, rIn.LowHighs.al, rIn.LowHighs.ah, rIn.LowHighs.sl, rIn.LowHighs.sh);
                        rIn.LowHighs = (nn.xl, nn.xh, nn.ml, nn.mh, nn.al, nn.ah, nn.sl, nn.sh);
                    }

                    ruleQueue.Enqueue((workflow.DefaultId, rIn));
                }


            }

            return sum.ToString();
        }
        private (long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh) GetNewRanges
            (char lh, string op, long rh, long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh)
        {
            switch (lh)
            {
                case 'x':
                    (xl, xh) = GetNewRange(op, rh, xl, xh);
                    break;
                case 'm':
                    (ml, mh) = GetNewRange(op, rh, ml, mh);
                    break;
                case 'a':
                    (al, ah) = GetNewRange(op, rh, al, ah);
                    break;
                case 's':
                    (sl, sh) = GetNewRange(op, rh, sl, sh);
                    break;
                default: throw new Exception("No!");
            }
            return (xl, xh, ml, mh, al, ah, sl, sh);
        }
        private (long low, long high) GetNewRange(string op, long n, long lo, long hi)
        {
            switch (op)
            {
                case ">":
                    lo = Math.Max(lo, n + 1);
                    break;
                case "<":
                    hi = Math.Min(hi, n - 1);
                    break;
                case ">=":
                    lo = Math.Max(lo, n);
                    break;
                case "<=":
                    hi = Math.Min(hi, n);
                    break;
                default:
                    throw new InvalidOperationException("Invalid operation");
            }
            return (lo, hi);
        }
        private bool RuleInputAccepted(ref Dictionary<string, Workflow> workflows, RuleInput ruleInput)
        {
            var currentWorkFlow = workflows.First(x => x.Key == "in").Value;
            while (true)
            {
                var e = currentWorkFlow.EvaluateRule(ruleInput);
                if (workflows.ContainsKey(e))
                {
                    currentWorkFlow = workflows[e];
                }
                else if (e == "R")
                {
                    return false;
                }
                else
                {
                    //Accepted
                    return true;
                }
            }
        }
        class Workflow
        {
            public string Id { get; set; }
            public List<(char lh, int rh, string op, string trueKey)> Rules { get; set; }
            public string DefaultId { get; set; }
            public string InputString { get; set; }
            public Workflow(string input)
            {
                var s1 = input.Split("{");
                var s2 = s1[1];
                var rulesString = s2.Split(",");
                var rules = rulesString.Take(rulesString.Length - 1).ToList();
                Rules = new List<(char lh, int rh, string op, string trueKey)>();
                foreach (var rule in rules)
                {
                    var lhrh = rule.Split(":");
                    var op = string.Empty;
                    var trueKey = lhrh[1];
                    var lh = '#';
                    var rh = 0;
                    if (lhrh[0].Contains("<"))
                    {
                        op = "<";
                        lh = lhrh[0].Split("<")[0].First();
                        rh = int.Parse(lhrh[0].Split("<")[1]);
                    }
                    if (lhrh[0].Contains(">"))
                    {
                        op = ">";
                        lh = lhrh[0].Split(">")[0].First();
                        rh = int.Parse(lhrh[0].Split(">")[1]);
                    }
                    Rules.Add((lh, rh, op, trueKey));
                }
                DefaultId = rulesString.Last().Trim('}');
                Id = s1[0];
                InputString = input;
            }
            public string EvaluateRule(RuleInput ruleInput)
            {
                foreach (var rule in Rules)
                {
                    (char lh, long rh, string op, string trueKey) = rule;
                    var lhValue = ruleInput.Values[lh];

                    if (op == "<" && lhValue < rh)
                    {
                        return trueKey;
                    }
                    if (op == ">" && lhValue > rh)
                    {
                        return trueKey;
                    }
                }
                return DefaultId;
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
                Values = new Dictionary<char, long>
                {
                    { 'x', x },
                    { 'm', m },
                    { 'a', a },
                    { 's', s }
                };
            }
            public RuleInput(int low, int high)
            {
                LowHighs = (low, high, low, high, low, high, low, high);
            }
            public RuleInput((long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh) newLowHigh)
            {
                LowHighs = newLowHigh;
            }

            public long x { get; set; }
            public long m { get; set; }
            public long a { get; set; }
            public long s { get; set; }

            public (long xl, long xh, long ml, long mh, long al, long ah, long sl, long sh) LowHighs { get; set; }

            public Dictionary<char, long> Values { get; set; }
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
            class Xmas
            {
                public long Xlow { get; set; }
                public long Xhigh { get; set; }
                public long Mlow { get; set; }
                public long Mhigh { get; set; }
                public long Alow { get; set; }
                public long Ahigh { get; set; }
                public long SHigh { get; set; }
                public long SLow { get; set; }

            }
        }

    }
}