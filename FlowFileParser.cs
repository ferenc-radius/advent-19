namespace advent_19;

public static class FlowFileParser
{
    public static ValueTuple<List<string>, List<Part>> Parse(string filePath)
    {
        var lines = File.ReadLines(filePath).ToArray();
        var offset = Array.FindIndex(lines, string.IsNullOrWhiteSpace);
        var tokens = ParseWorkflows(lines, offset);
        var parts = ParseParts(lines, offset);
 
        return new ValueTuple<List<string>, List<Part>>(tokens.ToList(), parts.ToList());
    }

    private static IEnumerable<Part> ParseParts(string[] lines, int offset)
    {
        var allParts = new List<Part>();
        foreach (var line in lines.Skip(offset+1))
        {
            var parts = line.Substring(1, line.Length-2).Split(",");
            var values = parts.Select(part => part.Split("=")).Select(p => Convert.ToInt32(p[1])).ToList();
            allParts.Add(new Part(values[0], values[1], values[2], values[3]));
        }
        return allParts;
    }

    private static IEnumerable<string> ParseWorkflows(string[] lines, int offset)
    {
        return lines.Take(offset).ToList();
    }
}