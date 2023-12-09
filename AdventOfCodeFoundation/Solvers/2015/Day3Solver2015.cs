using AdventOfCodeFoundation.IO;
namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2015/12/3")]
    internal class Day3Solver2015 : ISolver
    {
        Dictionary<char, (int x, int y)> instructionsMap = new Dictionary<char, (int x, int y)>
        {
            {'>',(1,0)},
            {'^',(0,1)},
            {'v',(0,-1)},
            {'<',(-1,0)},
        };
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var instructions = raw.ToList();
            (int x, int y) hPosition = (0, 0);
            var hashHouse = new HashSet<(int x, int y)>() { hPosition };
            foreach (var instruction in instructions)
            {
                var v = instructionsMap[instruction];
                hPosition.x = hPosition.x + v.x;
                hPosition.y = hPosition.y + v.y;
                hashHouse.Add(hPosition);
            }
            return hashHouse.Count().ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var instructions = raw.ToList();
            var pos1 = (0, 0);
            var pos2 = (0, 0);
            var hashHouse = new HashSet<(int x, int y)>() { pos1 };
            for (var i = 0; i < instructions.Count() - 1; i+=2)
            {
                pos1 = GetNewPosition(pos1, instructionsMap[instructions[i]]);
                pos2 = GetNewPosition(pos2, instructionsMap[instructions[i + 1]]);
                hashHouse.Add(pos1);
                hashHouse.Add(pos2);
            }
            return hashHouse.Count().ToString();
        }
        private (int x, int y) GetNewPosition((int x, int y) sPos, (int x, int y) insPos)
        {
            (int x, int y) newPos = sPos;
            newPos.x = newPos.x + insPos.x;
            newPos.y = newPos.y + insPos.y;
            return newPos;
        }
    }
}