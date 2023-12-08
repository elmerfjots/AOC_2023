using AdventOfCodeFoundation.IO;
namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/8")]
    internal class Day8Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");

            var instructions = rows.First();
            var nodesDict = new Dictionary<string, (string left, string right)>();
            var instructionsQueue = new Queue<string>();
            foreach (var n in rows.Skip(2))
            {
                var nodeKey = n.Split(" = ").First();
                var value = n.Split(" = ").Last();
                nodesDict.Add(nodeKey, StringToTupleValue(value));
            }
            foreach (var i in instructions.Select(x => x))
            {
                instructionsQueue.Enqueue(i.ToString());
            }
            var currentKey = "AAA";
            var steps = 0;
            while (currentKey != "ZZZ")
            {
                var curValue = nodesDict[currentKey];

                var instruction = instructionsQueue.Dequeue();

                if (instruction == "L")
                {
                    currentKey = curValue.left;
                }
                if (instruction == "R")
                {
                    currentKey = curValue.right;
                }
                instructionsQueue.Enqueue(instruction);
                steps++;
            }
            return steps.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n");
            var instructions = rows.First();
            var nodesDict = new Dictionary<string, (string left, string right)>();
            var instructionsQueue = new Queue<string>();
            foreach (var n in rows.Skip(2))
            {
                var nodeKey = n.Split(" = ").First();

                var value = n.Split(" = ").Last();
                nodesDict.Add(nodeKey, StringToTupleValue(value));
            }
            foreach (var i in instructions.Select(x => x))
            {
                instructionsQueue.Enqueue(i.ToString());
            }
            var currentKeys = nodesDict.Keys.Where(k => k[2] == 'A').ToList();
            long steps = 1;

            foreach (var key in currentKeys)
            {
                var lKey = key;
                long cnt = 0;
                while (!lKey.EndsWith("Z"))
                {
                    var instruction = instructionsQueue.Dequeue();
                    lKey = instruction == "L" ? nodesDict[lKey].left : nodesDict[lKey].right;
                    cnt++;
                    instructionsQueue.Enqueue(instruction);
                }
                // determine smallest common multiplier
                var (a, b) = steps > cnt ? (steps, cnt) : (cnt, steps);
                var lcm = LCM(a, b);
                steps = lcm;
            }
            return steps.ToString();
        }
        //https://stackoverflow.com/questions/13569810/least-common-multiple
        private long GCF(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        private long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }
        public (string left, string right) StringToTupleValue(string input)
        {
            var split = input.Split(", ");
            return (split[0].Replace("(", "").Trim(), split[1].Replace(")", "").Trim());
        }
        class Node
        {

            public Node(string input)
            {

            }
        }
    }
}