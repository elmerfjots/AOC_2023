using AdventOfCodeFoundation.IO;
using AdventOfCodeFoundation.Solvers;
using System.Reflection;

namespace AdventOfCodeFoundation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (CheckHelp(args))                         
                return;

            var challengeDate = GetChallengeDate(args);
            var solvers = GetSolvers(challengeDate);

            foreach (var solver in solvers)
                await solver.Run(challengeDate);            
        }

        private static DateOnly GetChallengeDate(string[] args)
        {          
            if (args.Any())
            {
                if (args.Length > 1)
                    Output.Warning($"Multiple arguments provided. Ignoring all arguments after '{args[0]}'...");

                return ParseArgument(args[0]);
            }                

            return DateOnly.FromDateTime(DateTime.Today);
        }

        private static bool CheckHelp(string[] args)
        {
            var shouldPrintHelp = ContainsHelp(args);
            if(shouldPrintHelp)
            {
                Output.Help();
            }
            return shouldPrintHelp;
        }

        private static bool ContainsHelp(string[] args)
        => args.Intersect(new[] { "?", "h", "-h", "--h", "help", "-help", "--help" }).Any();        

        private static DateOnly ParseArgument(string arg)
        {
            var date = TryParseInt(arg) ?? TryParseDateOnly(arg) ?? TryParseDateTime(arg);
            if (date.HasValue)
                return date.Value;

            throw new ArgumentException($"Could not parse argument {arg}.");
        }

        private static DateOnly? TryParseInt(string arg)
        {
            var today = DateTime.Today;

            return (int.TryParse(arg, out var day) && day > 0 && day < 31)
                ? new DateOnly(today.Year, today.Month, day)
                : null;
        }

        private static DateOnly? TryParseDateOnly(string arg)
            => DateOnly.TryParse(arg, out var date) ? date : null;

        private static DateOnly? TryParseDateTime(string arg)
            => DateTime.TryParse(arg, out var datetime) ? DateOnly.FromDateTime(datetime) : null;


        private static IEnumerable<ISolver> GetSolvers(DateOnly challengeDate)
        {
            var solvers = GetAllSolverImplementations()
                .Where(st => st.GetCustomAttribute<Solves>()?.challengeDate == challengeDate)
                .Select(st => (ISolver)Activator.CreateInstance(st)!);

            Output.Line($"Found {solvers.Count()} solvers for {challengeDate}...");

            return solvers;
        }

        private static IEnumerable<Type> GetAllSolverImplementations()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && typeof(ISolver).IsAssignableFrom(t));
        }
    }
}