using System.Text.RegularExpressions;

namespace advent_19;

public class FlowFileParser
{
    public Dictionary<string, FlowFunction> Functions { get; private set; }
    public List<Part> Parts { get; private set; }

    public FlowFileParser(string filePath)
    {
        Functions = new Dictionary<string, FlowFunction>();
        Parts = new List<Part>();
        var lines = File.ReadAllLines(filePath);
        var rowIndex = ParseFlowFunctions(lines);
        ParseParts(lines, rowIndex);
    }

    private int ParseFlowFunctions(string[] lines)
    {
        Functions = new Dictionary<string, FlowFunction>();

        foreach (var (line, rowIndex) in lines.Select((value, i) => (value, i)))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                var tokens = Tokenizer.Tokenizer.Tokenize(line);
                var flowFunction = new FlowFunction(tokens);
                Functions.Add(flowFunction.GetFunctionName(), flowFunction);
            }
            else
            {
                // return index where the parts start.
                return rowIndex + 1;
            }
        }

        return 0;
    }

    private void ParseParts(string[] lines, int offset)
    {
        Parts = new List<Part>();
        foreach (var line in lines.Skip(offset))
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
            Parts.Add(new Part(x, m, a, s));
        }
    }

    
}