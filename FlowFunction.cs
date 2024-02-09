using System.Diagnostics;
using advent_19.Tokenizer;

namespace advent_19;

public class FlowFunction(Stack<Token> tokenStack)
{
    [Conditional("DEBUG")]
    private void Debug(string name)
    {
        Console.Write(name + " -> ");
    }
    
    public bool Execute(string name, Part p, Dictionary<string, FlowFunction> functions)
    {
        Debug(name);
        var clonedStack = new Stack<Token>(new Stack<Token>(tokenStack));
        
        while (clonedStack.Count > 0)
        {
            var token = clonedStack.Pop();
            switch (token.Type)
            {
                case TokenType.PartS:
                case TokenType.PartM:
                case TokenType.PartA:
                case TokenType.PartX:
                    var success = ExecutePart(token.Type, p, clonedStack);
                    // if value is false we drop the if statement and condition.
                    if (!success)
                    {
                        clonedStack.Pop();
                        clonedStack.Pop();
                    }
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

    private static bool ExecutePart(TokenType tokenType, Part part, Stack<Token> clonedStack)
    {
        var partValue = tokenType switch
        {
            TokenType.PartX => part.X,
            TokenType.PartM => part.M,
            TokenType.PartA => part.A,
            TokenType.PartS => part.S,
            _ => throw new ArgumentOutOfRangeException()
        };

        return Compare(clonedStack.Pop(), clonedStack.Pop(), partValue);
    }
    
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

    public string GetFunctionName()
    {
        return tokenStack.First().Value;
    }
}