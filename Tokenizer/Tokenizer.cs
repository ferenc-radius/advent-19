using System.Text;
using System.Text.RegularExpressions;

namespace advent_19.Tokenizer;

public static class Tokenizer
{
    private static readonly List<(TokenType, object)> Defs =
    [
        (TokenType.SmallerThen, "<"),
        (TokenType.BiggerThen, ">"),
        (TokenType.Else, ","),
        (TokenType.If, ":"),
        (TokenType.Function, new Regex("^[a-z]{2,3}", RegexOptions.Compiled)),
        (TokenType.Number, new Regex(@"^\d+", RegexOptions.Compiled)),
        (TokenType.PartX, "x"),
        (TokenType.PartM, "m"),
        (TokenType.PartA, "a"),
        (TokenType.PartS, "s"),
        (TokenType.Accepted, "A"),
        (TokenType.Rejected, "R"),
    ];

    public static Stack<(TokenType, string)> Tokenize(string rule)
    {
        var tokens = new List<(TokenType, string)>();
        var remBuilder = new StringBuilder(rule);
        while (remBuilder.Length > 0)
        {
            var foundToken = false;
            foreach (var (tokenType, pattern) in Defs)
            {
                if (TryMatchPattern(remBuilder, pattern, out var value))
                {
                    tokens.Add((tokenType, value));
                    foundToken = true;
                    break;
                }
            }

            if (!foundToken && remBuilder.Length > 0)
            {
                remBuilder.Remove(0, 1);
            }
        }
 
        return new Stack<(TokenType, string)>(tokens.AsEnumerable().Reverse());
    }

    private static bool TryMatchPattern(StringBuilder input, object pattern, out string value)
    {
        switch (pattern)
        {
            case string strPattern:
                if (input.Length >= strPattern.Length && 
                    input.ToString(0, strPattern.Length) == strPattern)
                {
                    value = strPattern;
                    input.Remove(0, strPattern.Length);
                    return true;
                }
                break;

            case Regex regexPattern:
                var match = regexPattern.Match(input.ToString());
                if (match.Success)
                {
                    value = match.Value;
                    input.Remove(0, match.Length);
                    return true;
                }
                break;
        }

        value = "";
        return false;
    }
}