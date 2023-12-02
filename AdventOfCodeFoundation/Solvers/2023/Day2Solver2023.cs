using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/2")]
    internal class Day2Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var games = raw.Split("\r\n").Select(x => new Game(x));
            var possibleGames = games
                .Where(x => x.Blues <= 14 && x.Reds <= 12 && x.Greens <= 13)
                .Sum(x => x.Id);
            return possibleGames.ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var games = raw.Split("\r\n").Select(x => new Game(x));
            var possibleGames = games
              .Select(x => x.Greens * x.Reds * x.Blues)
              .Sum();
            return possibleGames.ToString();
        }
    }
    class Game
    {
        public int Id { get; set; }
        public int Reds { get; set; } = -1;
        public int Blues { get; set; } = -1;
        public int Greens { get; set; } = -1;

        public Game(string gameRow)
        {
            var gameSplit = gameRow.Split(": ");
            Id = int.Parse(gameSplit[0].Replace("Game ", ""));

            var sets = gameSplit.Last().Split("; ");
            foreach (var set in sets)
            {
                var s = set.Split(", ");
                foreach (var s2 in s)
                {
                    var digit = int.Parse(s2.Split(" ").First().Trim());
                    var color = s2.Split(" ").Last();
                    if (color.Equals("red") && digit > Reds) { Reds = digit; }
                    if (color.Equals("green") && digit > Greens) { Greens = digit; }
                    if (color.Equals("blue") && digit > Blues) { Blues = digit; }
                }
            }
        }
    }
}