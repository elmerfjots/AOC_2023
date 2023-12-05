using AdventOfCodeFoundation.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/4")]
    internal class Day4Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var cards = raw.Split("\r\n").Select(x => new ScratchCard(x));
            return cards.Sum(x => x.Points).ToString();
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var cards = raw.Split("\r\n").Select(x => new ScratchCard(x));
            var cardDict = cards.ToDictionary(x => x.Id);

            var wonCards = new List<ScratchCard>();
            var cardsToProcess = new Queue<ScratchCard>();
            foreach (var c in cards)
            {
                cardsToProcess.Enqueue(c);
                wonCards.Add(c);
            }
            while (cardsToProcess.Any(x => x.IsProcessed == false))
            {
                var card = cardsToProcess.Dequeue();
                for (int i = 1; i <= card.MatchingNumbers.Count; i++)
                {
                    var cardNumber = card.CardNumber;
                    if (cardDict.ContainsKey($"{cardNumber + i}") == false)
                    {

                        throw new Exception($"Key {cardNumber + i} was not present...");
                    }
                    cardsToProcess.Enqueue(cardDict[$"{cardNumber + i}"]);
                    wonCards.Add(card);
                }
            }
            return wonCards.Count.ToString();
        }
        private class ScratchCard
        {
            public List<int> WinningNumbers { get; set; }
            public List<int> Numbers { get; set; }
            public bool IsProcessed { get; set; } = false;
            public List<int> MatchingNumbers { get; set; }
            public string Id { get; set; }
            public int CardNumber { get; set; }
            public int Points { get; set; }
            public List<ScratchCard> Copies { get; set; }
            public ScratchCard(string x)
            {
                var fSplit = x.Split(": ");
                Id = fSplit[0].Trim().Replace("Card", "").Trim();
                CardNumber = int.Parse(Id.Split(" ").Last().Trim());
                var s = fSplit.Last().Split(" | ");
                WinningNumbers = s.First().Split(" ").Where(x => string.IsNullOrEmpty(x) == false).Select(n => int.Parse(n)).ToList();
                Numbers = s.Last().Split(" ").Where(x => string.IsNullOrEmpty(x) == false).Select(n => int.Parse(n.Trim())).ToList();

                MatchingNumbers = Numbers.Intersect(WinningNumbers).ToList();

                foreach (var row in MatchingNumbers)
                {
                    if (MatchingNumbers.Count == 1) { Points = 1; continue; }
                    var d = 1;
                    for (var i = 1; i < MatchingNumbers.Count; i++)
                    {
                        d = d * 2;
                    }
                    Points = d;
                }

            }
            public override string ToString()
            {
                return Id.ToString();
            }
        }
    }
}