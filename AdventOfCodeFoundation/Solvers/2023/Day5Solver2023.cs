using AdventOfCodeFoundation.IO;

namespace AdventOfCodeFoundation.Solvers._2023
{
    [Solves("2023/12/5")]
    internal class Day5Solver2023 : ISolver
    {
        public async Task<string> SolvePartOne(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").Where(x => !x.Equals("")).ToList();
            var seeds = rows.First().Split(": ").Last().Trim().Split(" ").Select(x => long.Parse(x)).ToList();
            var minLocation = GetMinLocation(seeds, ParseMaps(rows));
            return minLocation.ToString();
        }
        public async Task<string> SolvePartTwo(Input input)
        {
            var raw = await input.GetRawInput();
            var rows = raw.Split("\r\n").Where(x => !x.Equals("")).ToList();
            var seedRaw = rows.First().Split(": ").Last().Trim().Split(" ").Select(long.Parse).ToList();
            var ranges = new Dictionary<long, List<long>>();
            var maps = ParseMaps(rows);
            for (var i = 0; i < seedRaw.Count; i += 2)
            {
                var start = seedRaw[i];
                var range = seedRaw[i + 1];
                ranges.Add(start, new List<long>());
                //seeds.Add(start);
                for (var k = start; k < start + range; k++)
                {
                    ranges[start].Add(k);
                }
            }

            (long initial, long minValue) minLocation = (-1, long.MaxValue);
            foreach (var s in ranges)
            {
                var l = GetMinLocation(s.Value, maps); ;
                minLocation = (minLocation.minValue > l) ? (s.Key, l) : minLocation;
            }

            return minLocation.ToString();
        }
        private List<Map> ParseMaps(List<string> rows)
        {
            var stsIdx = rows.IndexOf("seed-to-soil map:");
            var stfIdx = rows.IndexOf("soil-to-fertilizer map:");
            var ftwIdx = rows.IndexOf("fertilizer-to-water map:");
            var wtlIdx = rows.IndexOf("water-to-light map:");
            var lttIdx = rows.IndexOf("light-to-temperature map:");
            var tthIdex = rows.IndexOf("temperature-to-humidity map:");
            var htlIdx = rows.IndexOf("humidity-to-location map:");
            var seedToSoilMap = new Map(rows.Skip(1 + stsIdx).Take((stfIdx - 1) - stsIdx).ToList());
            var soilToFertilizerMap = new Map(rows.Skip(1 + stfIdx).Take((ftwIdx - 1) - stfIdx).ToList());
            var fertilizerToWaterMap = new Map(rows.Skip(1 + ftwIdx).Take((wtlIdx - 1) - ftwIdx).ToList());
            var waterToLightMap = new Map(rows.Skip(1 + wtlIdx).Take((lttIdx - 1) - wtlIdx).ToList());
            var lightToTemperatureMap = new Map(rows.Skip(1 + lttIdx).Take((tthIdex - 1) - lttIdx).ToList());
            var temperatureToHumidityMap = new Map(rows.Skip(1 + tthIdex).Take((htlIdx - 1) - tthIdex).ToList());
            var himidityToLocationMap = new Map(rows.Skip(1 + htlIdx).Take((rows.Count - 1) - htlIdx).ToList());

            return new List<Map> { seedToSoilMap, soilToFertilizerMap, fertilizerToWaterMap, waterToLightMap, lightToTemperatureMap, temperatureToHumidityMap, himidityToLocationMap };
        }
        private long GetMinLocation(List<long> seeds, List<Map> maps)
        {
            var minLocation = long.MaxValue;
            foreach (var s in seeds)
            {
                var l1 = maps[0].GetValueForLong(s);
                var l2 = maps[1].GetValueForLong(l1);
                var l3 = maps[2].GetValueForLong(l2);
                var l4 = maps[3].GetValueForLong(l3);
                var l5 = maps[4].GetValueForLong(l4);
                var l6 = maps[5].GetValueForLong(l5);
                var l7 = maps[6].GetValueForLong(l6);
                minLocation = (l7 < minLocation) ? l7 : minLocation;
            }
            return minLocation;
        }
        private class Map
        {
            public List<MapNode> Values { get; set; }
            public Map(List<string> stringMap)
            {
                Values = new List<MapNode>();
                foreach (var s in stringMap)
                {
                    var destinationRangeStart = long.Parse(s.Split(" ").First());
                    var sourceRangeStart = long.Parse(s.Split(" ")[1]);

                    var rangeLength = long.Parse(s.Split(" ").Last());

                    var destinationRangeMax = destinationRangeStart + rangeLength - 1;
                    var sourceRangeMax = sourceRangeStart + rangeLength - 1;

                    Values.Add(new MapNode((sourceRangeStart, sourceRangeMax), (destinationRangeStart, destinationRangeMax)));
                }
            }
            public long GetValueForLong(long l)
            {
                foreach (var v in Values)
                {
                    if (l >= v.SourceMinMax.min && l <= v.SourceMinMax.max)
                    {
                        var dest = Math.Abs(l - v.SourceMinMax.min);

                        var d = v.DestinationMinMax.min + dest;

                        return d;
                    }
                }

                return l;
            }
        }
        class MapNode
        {
            public (long min, long max) DestinationMinMax { get; set; }
            public (long min, long max) SourceMinMax { get; set; }
            public MapNode((long min, long max) sourceMinMax, (long min, long max) destMinMax)
            {
                DestinationMinMax = destMinMax;
                SourceMinMax = sourceMinMax;
            }
        }
    }
}