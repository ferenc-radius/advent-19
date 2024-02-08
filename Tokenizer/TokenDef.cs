using System.Text.RegularExpressions;

namespace advent_19.Tokenizer;

public class TokenDef(TokenType type, string value)
{
    public TokenType Type { get; } = type;
    private readonly Regex _regex = new(value);

    public Match Match(string part)
    {
        return _regex.Match(part);
    }
}