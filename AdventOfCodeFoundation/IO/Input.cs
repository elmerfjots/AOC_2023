namespace AdventOfCodeFoundation.IO
{
    public class Input
    {
        private readonly string file;

        public Input(string fileName)
        {
            file = fileName;
        }

        public Input(DateOnly challengeDate)
        {
            file = $"../../../Inputs/{challengeDate.Year}/{challengeDate.Day:D2}/input.txt";
        }

        public async Task<string> GetRawInput()
        {
            return await File.ReadAllTextAsync(file);
        }

        public async Task<IEnumerable<string>> GetInputLines()
        {
            return await File.ReadAllLinesAsync(file);
        }
    }
}
