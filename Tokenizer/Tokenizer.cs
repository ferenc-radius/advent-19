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
        (TokenType.Function, new Regex("^[a-z]{2,3}")),
        (TokenType.Number, new Regex(@"^\d+")),
        (TokenType.PartX, "x"),
        (TokenType.PartM, "m"),
        (TokenType.PartA, "a"),
        (TokenType.PartS, "s"),
        (TokenType.Accepted, "A"),
        (TokenType.Rejected, "R"),
        (TokenType.Start, "{"),
        (TokenType.Stop, "}")
    ];

    public static Stack<Token> Tokenize(string rule)
    {
        var tokens = new List<Token>();
        var remBuilder = new StringBuilder(rule);
        while (remBuilder.Length > 0)
        {
            var foundToken = false;
            foreach (var (tokenType, pattern) in Defs)
            {
                if (TryMatchPattern(remBuilder, pattern, out var value))
                {
                    tokens.Add(new Token(tokenType, value));
                    foundToken = true;
                    break;
                }
            }

            if (!foundToken && remBuilder.Length > 0)
            {
                remBuilder.Remove(0, 1);
            }
        }
 
        return new Stack<Token>(tokens.AsEnumerable().Reverse());
    }

    private static bool TryMatchPattern(StringBuilder input, object pattern, out string value)
    {
        switch (pattern)
        {
            case string strPattern when input.ToString().StartsWith(strPattern):
                value = strPattern;
                input.Remove(0, strPattern.Length);
                return true;

            case Regex regexPattern:
                var match = regexPattern.Match(input.ToString());
                if (match.Success)
                {
                    value = match.Value.Trim();
                    input.Remove(0, match.Value.Length);
                    return true;
                }
                break;
        }

        value = "";
        return false;
    }
}