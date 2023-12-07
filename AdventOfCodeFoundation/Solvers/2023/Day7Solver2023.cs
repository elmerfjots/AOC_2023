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
        public static Dictionary<string, string> SortKeyDictionary = new Dictionary<string, string>()
        {
            {"A","A"},
            {"K","B"},
            {"Q","C"},
            {"J","D"},
            {"T","E"},
            {"9","F"},
            {"8","G"},
            {"7","H"},
            {"6","I"},
            {"5","J"},
            {"4","K"},
            {"3","L"},
            {"2","M"},

        };
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var hands = raw.Split("\r\n")
                .Select(x => new CardHand(x))
                .OrderByDescending(x => x.Score)
                .GroupBy(x => x.Score);
            var sum = GetSumOfHands(hands);

            return sum.ToString();
        }



        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            SortKeyDictionary["J"] = "N";
            var hands = raw.Split("\r\n")
               .Select(x => new CardHand(x, true)).Select(x => x.GetBestSubstitute()).ToList();
            var allCards = new List<CardHand>();
            //foreach (var h in hands)
            //{
            //    if (h.Substitutions.Any())
            //    {
            //        //Mangler en orderby score
            //        var bestSubstitution = h.Substitutions.OrderByDescending(x => x.Score).First();
            //        allCards.Add(bestSubstitution);
            //    }
            //    else
            //    {
            //        allCards.Add(h);
            //    }

            //}
            var handsg = hands.OrderByDescending(x => x.Score)
                .GroupBy(x => x.Score);
            var ranks = new Stack<CardHand>();
            long sum = GetSumOfHands(handsg);

            return sum.ToString();
        }
        private long GetSumOfHands(IEnumerable<IGrouping<int, CardHand>> hands)
        {
            long sum = 0;

            var ranks = new Stack<CardHand>();
            foreach (var group in hands)
            {
                if (group.ToList().Count == 1)
                {
                    ranks.Push(group.First());
                    continue;
                }
                var sorted = group.OrderBy(x => x.OriginalSortKey).ToList();
                foreach (var card in sorted)
                {
                    ranks.Push(card);
                }
            }
            var c = 1;
            while (ranks.Any())
            {
                var card = ranks.Pop();
                sum += card.Bid * c;
                c++;
            }
            return sum;
        }

        class CardHand
        {
            public List<Card> Cards { get; set; }
            public int Bid { get; set; }
            public string SortKey { get; set; }

            public string OriginalSortKey { get; set; }
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

            public List<CardHand> Substitutions { get; set; }
            public CardHand() { }
            public CardHand(string row, bool jokerChange = false)
            {
                Substitutions = new List<CardHand>();
                Bid = int.Parse(row.Split(" ").Last().Trim());
                Cards = row.Split(" ").First().Trim().Select(x => new Card(x.ToString(), jokerChange)).ToList();

                //[x3][x2][x][J][y][y2][y3]
                //Highest hand right, highest hand left

                //Contains Joker
                if (jokerChange && Cards.Any(x => x.OriginalValue.Equals("J")))
                {

                    CreateSubstitutions();

                }
                EvaluateCards(Cards);
            }

            public CardHand(List<Card> subCards, int bid)
            {
                Bid = bid;
                Cards = subCards;
                EvaluateCards(Cards);
            }

            private void CreateSubstitutions()
            {

                var idxOfJ = Cards.Select((x, i) => new { x, i })
                  .Where(x => x.x.Value == "J")
                  .Select(x => x.i)
                  .ToList();
                var dCards = Cards.DistinctBy(x => x.Value).ToList().Where(x => x.Value != "J").ToList();
                foreach (var dc in dCards)
                //IT IS COPYING THE VARIABLE IN THE ARRAY :S
                {
                    var subCards = new List<Card>(Cards);
                    foreach (var jidx in idxOfJ)
                    {
                        var cc = new Card(dc);

                        cc.OriginalValue = "J";
                        cc.ReEvaluateCard(dc.Value, true);
                        subCards[jidx] = cc;

                    }
                    var c = new CardHand(subCards, Bid);
                    c.EvaluateCards(subCards);
                    Substitutions.Add(c);
                }
            }
            public CardHand GetBestSubstitute()
            {
                if (Substitutions.Any())
                {
                    //Mangler en orderby score
                    var bestSubstitution = Substitutions.OrderByDescending(x => x.Score).First();
                    return bestSubstitution;
                }
                return this;
            }
            public void EvaluateCards(List<Card> cards)
            {
                var cardGroups = cards.OrderBy(x => x.IntValue).GroupBy(x => x.Value).ToList();
                FiveOfAKind = cardGroups.Count == 1;
                FourOfAKind = cardGroups.Count == 2 && cardGroups.Any(x => x.ToList().Count == 4);
                FullHouse = cardGroups.Count == 2 && cardGroups.Any(x => x.ToList().Count == 3);
                ThreeOfAKind = !FullHouse && cardGroups.Any(x => x.ToList().Count == 3);
                TwoPair = cardGroups.Count == 3;
                OnePair = cardGroups.Count == 4;
                HighCard = cardGroups.Count == 5;
                SortKey = string.Join("", Cards.Select(x => SortKeyDictionary[x.Value]));
                OriginalSortKey = string.Join("", Cards.Select(x => SortKeyDictionary[x.OriginalValue]));
            }

            public override string ToString()
            {
                return string.Join("", Cards) + " " + Bid;
            }
        }
        class Card
        {
            public string Value { get; set; }
            public int IntValue { get; set; }

            public string OriginalValue { get; set; }
            public Card() { }

            public Card(Card copy)
            {
                Value = copy.Value;
                IntValue = copy.IntValue;
                OriginalValue = copy.Value;

            }
            public Card(string input, bool jokerChange = false)
            {

                Value = input;
                OriginalValue = Value;
                ReEvaluateCard(input, jokerChange);

            }
            public void ReEvaluateCard(string input, bool jokerChange)
            {
                var s = input.Replace("A", "14");
                s = s.Replace("K", "13");
                s = s.Replace("Q", "12");

                s = jokerChange ? s.Replace("J", "1") : s.Replace("J", "11");

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