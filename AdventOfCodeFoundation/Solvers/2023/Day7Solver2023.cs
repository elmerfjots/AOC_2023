using AdventOfCodeFoundation.IO;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Xml.Linq;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/7")]
    internal class Day7Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").ToList();
            var cardHands = new List<CardHand>();
            foreach (var row in rows)
            {

                cardHands.Add(new CardHand(row));
            }
            var winningHands =  cardHands.OrderBy(x=>x.Score);

            return "";
        }

        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();

            return "";
        }

        class CardHand
        {
            public List<Card> Cards { get; set; }
            public int Bid { get; set; }
            public int Score
            {
                get
                {
                    if (FiveOfAKind) { return 100; }
                    if (FourOfAKind) { return 90; }
                    if (FullHouse) { return 80; }
                    if (ThreeOfAKind) { return 70; }
                    if (TwoPair) { return 60; }
                    if (OnePair) { return 50; }
                    if (HighCard) { return 40; }
                    return -1;
                }
            }
            public bool FiveOfAKind { get; set; } // 1
            public bool FourOfAKind { get; set; } // 2

            public bool FullHouse { get; set; } // 3
            public bool ThreeOfAKind { get; set; } //4
            public bool TwoPair { get; set; }
            public bool OnePair { get; set; }
            public bool HighCard { get; set; }
            public CardHand(string row)
            {
                Bid = int.Parse(row.Split(" ").Last().Trim());
                Cards = new List<Card>();
                row = row.Split(" ").First().Trim();
                foreach (var item in row)
                {
                    Cards.Add(new Card(item.ToString()));
                }
                var cardGroups = Cards.OrderBy(x => x.IntValue).GroupBy(x => x.Value).ToList();
                FiveOfAKind = cardGroups.Count == 1;
                FourOfAKind = cardGroups.Count == 2 && cardGroups.Any(x => x.ToList().Count == 4);
                FullHouse = cardGroups.Count == 2 && cardGroups.Any(x => x.ToList().Count == 3);
                ThreeOfAKind = !FullHouse && cardGroups.Any(x => x.ToList().Count == 3);
                TwoPair = cardGroups.Count == 3;
                OnePair = cardGroups.Count == 4;
                HighCard = cardGroups.Count == 5;
            }
            public override string ToString()
            {
                return string.Join("", Cards) + " " + Bid;
            }
            //bool CheckFullHouse(List<IGrouping<string,Card>> groups)
            //{
            //    if (groups.Count != 2) { return false; }
            //    if(groups.Any(x => x.ToList().Count == 3) == false) { return false; }

            //    foreach (var group in groups)
            //    {
            //        if(group.ToList().Count == 3) { continue; }

            //    }
            //}

        }
        class Card
        {
            public string Value { get; set; }
            public int IntValue { get; set; }

            public Card(string input)
            {

                Value = input;
                var s = input.Replace("A", "14");
                s = s.Replace("K", "13");
                s = s.Replace("Q", "12");
                s = s.Replace("J", "11");
                s = s.Replace("T", "10");
                IntValue = int.Parse(s);
            }
            public override bool Equals(object obj)
            {
                var other = obj as Card;
                if (other == null)
                    return false;
                return Value.Equals(other.Value);
            }
            public override string ToString()
            {
                return Value;
            }
        }
    }
}