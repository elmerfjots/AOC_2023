using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
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
            var sum = ruleInputs.Where(x => RuleInputAccepted(ref workflows, x)).Sum(x => x.Score);
            return sum.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rawSplit = raw.Split("\r\n\r\n");
            var workflows = rawSplit[0].Split("\r\n").Select(x => new Workflow(x)).ToDictionary(x => x.Id);
            var rulesInputString = rawSplit[1].Split("\r\n");
            var ruleInputs = rulesInputString.Select(x => new RuleInput(1, 4000)).ToList();

            var q = new Queue<(string key, int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh)>();
            q.Enqueue(("in", 1, 4000, 1, 4000, 1, 4000, 1, 4000));
            long sum = 0;
            while (q.Any())
            {
                (string key, int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh) = q.Dequeue();
                if (xl > xh || ml > mh || al > ah || sl > sh) { continue; }
                if (key == "A")
                {
                    sum += (xh - (long)xl + 1) * (mh - (long)ml + 1) * (ah - (long)al + 1) * (sh - (long)sl + 1);
                    continue;
                }
                else if (key == "R") { continue; }
                else
                {
                    var workflow = workflows[key];
                    foreach (var rule in workflow.Rules)
                    {
                        var n = GetNewRanges(rule.lh, rule.op, rule.rh, xl, xh, ml, mh, al, ah, sl, sh);
                        q.Enqueue((rule.trueKey, n.xl, n.xh, n.ml, n.mh, n.al, n.ah, n.sl, n.sh));

                        //Negation of operators
                        var newOp = rule.op == ">" ? "<=" : ">=";

                        var nn = GetNewRanges(rule.lh, newOp, rule.rh, xl, xh, ml, mh, al, ah, sl, sh);
                        xl = nn.xl; xh = nn.xh; ml = nn.ml; mh = nn.mh; al = nn.al; ah = nn.ah; sl = nn.sl; sh = nn.sh;
                    }

                    q.Enqueue((workflow.DefaultId, xl, xh, ml, mh, al, ah, sl, sh));
                }


            }

            return sum.ToString();
        }
        private (int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh) GetNewRanges
            (char lh, string op, int rh, int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh)
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
        private (int low, int high) GetNewRange(string op, int n, int lo, int hi)
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

            public long x { get; set; }
            public long m { get; set; }
            public long a { get; set; }
            public long s { get; set; }

            public (int xl, int xh, int ml, int mh, int al, int ah, int sl, int sh) LowHighs { get; set; }

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
        }

    }
}