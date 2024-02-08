using advent_19.Tokenizer;

namespace advent_19;

public class FlowFunction(Stack<Token> tokenStack)
{
    private static bool Compare(Token compare, Token value, int var)
    {
        var conversionSuccess = int.TryParse(value.Value, out var toCompare);
        if (!conversionSuccess)
        {
            throw new InvalidCastException("value not a number: " + value);
        }
        
        return compare.Type switch
        {
            TokenType.SmallerThen => var < toCompare,
            TokenType.BiggerThen => var > toCompare,
            _ => throw new InvalidOperationException("Unrecognized token")
        };
    } 
    
    public bool Execute(string name, Part p, Dictionary<string, FlowFunction> functions)
    {
        Console.Write(name + " -> ");
        var clonedStack = new Stack<Token>(new Stack<Token>(tokenStack));
        
        while (clonedStack.Count > 0)
        {
            var token = clonedStack.Pop();
            if (token.Type == TokenType.Stop)
            {
                return false;
            }

            switch (token.Type)
            {
                case TokenType.PartS:
                    ExecutePart(p.S, clonedStack);
                    break;
                
                case TokenType.PartM:
                    ExecutePart(p.M, clonedStack);
                    break;
                
                case TokenType.PartA:
                    ExecutePart(p.A, clonedStack);
                    break;
                
                case TokenType.PartX:
                    ExecutePart(p.X, clonedStack);
                    break;

                case TokenType.If:
                case TokenType.Else:
                    var tokenAhead = clonedStack.Peek();
                    if (tokenAhead.Type == TokenType.Function)
                    {
                        var function = clonedStack.Pop();
                        return functions[function.Value].Execute(function.Value, p, functions);
                    }
                    break;

                case TokenType.Accepted:
                    return true;

                case TokenType.Rejected:
                    return false;
            }
        }

        return false;
    }

    private static void ExecutePart(int partValue, Stack<Token> clonedStack)
    {
        var lastFunctionState = Compare(clonedStack.Pop(), clonedStack.Pop(), partValue);
        
        // if value is false we drop the if statement and condition.
        if (!lastFunctionState)
        {
            clonedStack.Pop();
            clonedStack.Pop();
        }
    }

    public string GetFunctionName()
    {
        return tokenStack.First().Value;
    }
}