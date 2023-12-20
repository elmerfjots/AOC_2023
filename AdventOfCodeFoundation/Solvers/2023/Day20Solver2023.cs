using AdventOfCodeFoundation.Common;
using AdventOfCodeFoundation.IO;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/20")]
    internal class Day20Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var inputRaw = raw.Split("\r\n");
            var modules = GetTempModules(inputRaw);
            InitialzeModules(ref modules, ref inputRaw, out _);
            var processOrder = new Queue<string>();
            long lowPulses = 0;
            long highPulses = 0;

            for (int i = 0; i < 1000; i++)
            {
                modules["broadcaster"].incomingPulses.Enqueue(("button", false));
                processOrder.Enqueue("broadcaster");

                while (processOrder.Any())
                {
                    var pulseDest = processOrder.Dequeue();
                    (long pulsesSent, bool pulseVal) = modules[pulseDest].DoPulse(ref modules, ref processOrder);

                    if (pulseVal) highPulses += pulsesSent;
                    else lowPulses += pulsesSent;
                }
            }
            return (highPulses * lowPulses).ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var inputRaw = raw.Split("\r\n");
            var modules = GetTempModules(inputRaw);
            InitialzeModules(ref modules, ref inputRaw, out string rx);
            var processOrder = new Queue<string>();
            var rxDests = modules.Values
                                    .Where(a => a.outputs.Contains(rx))
                                    .ToDictionary(m => m.Key, m => 0L);

            foreach (var m in modules.Values) m.Reset();
            var cnt = 0L;
            while (true)
            {
                cnt++;
                modules["broadcaster"].incomingPulses.Enqueue(("button", false));
                processOrder.Enqueue("broadcaster");

                while (processOrder.Any())
                {
                    var pulseDest = processOrder.Dequeue();
                    (_, bool isHighPulse) = modules[pulseDest].DoPulse(ref modules, ref processOrder);
                    if (rxDests.ContainsKey(pulseDest) && rxDests[pulseDest] == 0 && isHighPulse)
                    {
                        rxDests[pulseDest] = cnt;
                    }
                }

                if (rxDests.Values.All(x => x != 0)) break;
            }
            var sum = rxDests.Values.Aggregate(1L, (acc, n) => acc = AOCMath.LCM(acc, n));
            return sum.ToString();
        }

        private void InitialzeModules(ref Dictionary<string, Module> modules, ref string[] inputRaw, out string rx)
        {
            rx = string.Empty;
            foreach (var s in inputRaw)
            {
                var lhrh = s.Split(" -> ");

                var type = lhrh[0].Contains("&") ? ModuleType.Conjunction :
                  lhrh[0].Contains("%") ? ModuleType.FlipFlop :
                  lhrh[0].Equals("broadcaster") ? ModuleType.Broadcast :
                  ModuleType.Unknown;

                var currentModule = GetKey(lhrh[0]);
                modules[currentModule].Type = type;
                foreach (var dest in lhrh[1].Split(", "))
                {
                    modules[currentModule].outputs.Add(dest);
                    modules[dest].lastReceivedPulses[currentModule] = false;
                    if (dest == "rx") rx = currentModule;
                }

            }
        }

        private Dictionary<string, Module> GetTempModules(string[] inputRaw)
        {
            var keys = new HashSet<string>(inputRaw.SelectMany(s => s.Split(" -> ")[1]
                                                                        .Split(", ")
                                                                        .Concat(new[] { GetKey(s.Split(" -> ")[0]) }
                                                                        )));

            return keys.Select(x => new Module()
            {
                Key = x
            }).ToDictionary(x => x.Key);
        }
        private string GetKey(string s)
        {
            return s.Equals("broadcaster") ? s.Substring(0) : s.Substring(1);
        }
        private class Module
        {
            public required string Key { get; set; }
            public ModuleType Type = ModuleType.Unknown;
            public bool flipFlopState = false;
            public Dictionary<string, bool> lastReceivedPulses = new();
            public Queue<(string source, bool pulse)> incomingPulses = new();
            public List<string> outputs = new();

            public void Reset()
            {
                flipFlopState = false;
                foreach (var k in lastReceivedPulses.Keys)
                {
                    lastReceivedPulses[k] = false;
                }
            }

            public (long pulsesSent, bool isHighPulse) DoPulse(ref Dictionary<string, Module> modules, ref Queue<string> processOrder)
            {
                long pulsesSent = 0;
                bool isHighPulse = false;

                if (incomingPulses.TryDequeue(out var p))
                {
                    (var src, var pulse) = p;
                    if (Type == ModuleType.FlipFlop)
                    {
                        if (!pulse)
                        {

                            flipFlopState = !flipFlopState;
                            foreach (var n in outputs)
                            {
                                modules[n].incomingPulses.Enqueue((Key, flipFlopState));
                                processOrder.Enqueue(n);
                                pulsesSent++;
                            }

                            isHighPulse = flipFlopState;
                        }
                    }
                    else if (Type == ModuleType.Conjunction)
                    {
                        lastReceivedPulses[src] = pulse;
                        isHighPulse = lastReceivedPulses.Values.Any(a => !a);

                        foreach (var n in outputs)
                        {
                            modules[n].incomingPulses.Enqueue((Key, isHighPulse));
                            processOrder.Enqueue(n);
                            pulsesSent++;
                        }
                    }
                    else if (Type == ModuleType.Broadcast)
                    {
                        isHighPulse = pulse;
                        foreach (var n in outputs)
                        {
                            modules[n].incomingPulses.Enqueue((Key, isHighPulse));
                            processOrder.Enqueue(n);
                            pulsesSent++;
                        }
                    }
                }
                return (pulsesSent, isHighPulse);
            }
        }

        private enum ModuleType
        {
            FlipFlop,
            Conjunction,
            Broadcast,
            Unknown
        }
    }
}