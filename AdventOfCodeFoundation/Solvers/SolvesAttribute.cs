namespace AdventOfCodeFoundation.Solvers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Solves : Attribute
    {
        public readonly DateOnly challengeDate;

        public Solves(string challengeDate)
        {
            this.challengeDate = DateOnly.Parse(challengeDate);
        }
    }
}
