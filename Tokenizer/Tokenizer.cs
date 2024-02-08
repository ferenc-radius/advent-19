namespace advent_19.Tokenizer;

public static class Tokenizer
{
    private static readonly List<TokenDef> Defs =
    [
        new TokenDef(TokenType.SmallerThen, "^<"),
        new TokenDef(TokenType.BiggerThen, "^>"),
        new TokenDef(TokenType.Else, "^,"),
        new TokenDef(TokenType.If, "^:"),
        new TokenDef(TokenType.Function, "^[a-z]{2,3}"),
        new TokenDef(TokenType.Number, @"^\d+"),
        new TokenDef(TokenType.PartX, "^x"),
        new TokenDef(TokenType.PartM, "^m"),
        new TokenDef(TokenType.PartA, "^a"),
        new TokenDef(TokenType.PartS, "^s"),
        new TokenDef(TokenType.Accepted, "^A"),
        new TokenDef(TokenType.Rejected, "^R"),
        new TokenDef(TokenType.Start, "^{"),
        new TokenDef(TokenType.Stop, "^}")
    ];

    public static Stack<Token> Tokenize(string rule)
    {
        var tokens = new List<Token>();
        var rem = rule;
        while (!string.IsNullOrWhiteSpace(rem))
        {
            var foundToken = false;
            foreach (var def in Defs)
            {
                var match = def.Match(rem);
                if (match.Success)
                {
                    tokens.Add(new Token(def.Type, match.Value.Trim()));
                    rem = rem.Substring(match.Value.Length);
                    foundToken = true;
                    break;
                }
            }

            if (!foundToken && rem.Length > 0)
            {
                rem = rem.Substring(1);
            }
        }
 
        return CreateStack(tokens);
    }

    private static Stack<Token> CreateStack(List<Token> tokens)
    {
        var tokensStack = new Stack<Token>();
        var count = tokens.Count;
        for (var i = count - 1; i >= 0; i--)
        {
            tokensStack.Push(tokens[i]);
        }

        return tokensStack;
    }
}