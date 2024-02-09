using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using advent_19.Tokenizer;

namespace advent_19;

public static class FlowFileParser
{
    public static ValueTuple<List<Stack<Token>>, List<Part>> Parse(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var offset = Array.FindIndex(lines, string.IsNullOrWhiteSpace);
        var tokens = ParseWorkflows(lines, offset);
        var parts = ParseParts(lines, offset);
 
        return new ValueTuple<List<Stack<Token>>, List<Part>>(tokens.ToList(), parts.ToList());
    }

    private static List<Part> ParseParts(string[] lines, int offset)
    {
        var parts = new ConcurrentBag<Part>();
        Parallel.ForEach( lines.Skip(offset+1), (line) =>
        {
            var match = Regex.Match(line, "x=(\\d+)?,m=(\\d+),a=(\\d+),s=(\\d+)");
            if (!match.Success)
            {
                throw new Exception("Failed to parse parts");
            }

            var x = Convert.ToInt32(match.Groups[1].Value);
            var m = Convert.ToInt32(match.Groups[2].Value);
            var a = Convert.ToInt32(match.Groups[3].Value);
            var s = Convert.ToInt32(match.Groups[4].Value);
            parts.Add(new Part(x, m, a, s));
        });

        return parts.ToList();
    }

    private static IEnumerable<Stack<Token>> ParseWorkflows(string[] lines, int offset)
    {
        var tokens = new List<Stack<Token>>();
        Parallel.ForEach( lines.Take(offset), (line) =>
        {
            lock (tokens)
            {
                tokens.Add(Tokenizer.Tokenizer.Tokenize(line));
            }
        });
    
        return tokens;
    }
}