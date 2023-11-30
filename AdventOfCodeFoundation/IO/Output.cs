namespace AdventOfCodeFoundation.IO
{
    public static class Output
    {
        public static void String(string str) => Console.Write($"{DateTime.Now.ToLongTimeString()}: {str}");
        public static void Line(string str) => Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {str}");
        public static void Warning(string str) => Line($"Warning: {str}");
        public static void Warning(string str, Exception e) => Line($"Warning: {str}. Exception: {e}");
        public static void Error(string str) => Line($"Warning: {str}");
        public static void Error(string str, Exception e) => Line($"Warning: {str}. Exception: {e}");

        public static void Help() => Console.Write(helptext);

        const string helptext = @"
**************************
Advent of Code Foundation
**************************
Usage:
<no argument> - Solves challenge based on current system date. Ex: '.\AdventOfCodeFoundation.exe'
<n> - Solves challenge for day <n> in the current year. Ex: '.\AdventOfCodeFoundation.exe 25'
<date> - Solves challenge for the given date. Ex: '.\AdventOfCodeFoundation.exe 25-12-2018'
               
Have fun!

";

    }
}
