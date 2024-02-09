using System.Diagnostics;
using advent_19.Tokenizer;

namespace advent_19;

public class FlowFunction(Stack<Token> tokenStack)
{
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

    public List<(TokenType, int)> FindValues(TokenType type)
    {
        if (type is not (TokenType.PartX or TokenType.PartM or TokenType.PartA or TokenType.PartS))
        {
            throw new InvalidOperationException("Invalid part type");
        }
        
        var clonedStack = new Stack<Token>(new Stack<Token>(tokenStack));
        var values = new List<(TokenType, int)>();

        while (clonedStack.Count > 0)
        {
            var token = clonedStack.Pop();

            if (token.Type == type)
            {
                var compareToken = clonedStack.Pop();
                var numberToken = clonedStack.Pop();
                var toCompare = ToCompare(numberToken);

                values.Add((compareToken.Type, toCompare));
            }
        }

        return values;
    }

    public string GetFunctionName()
    {
        return tokenStack.First().Value;
    }

    private static bool ExecutePart(TokenType tokenType, Part part, Stack<Token> stack)
    {
        var partValue = tokenType switch
        {
            TokenType.PartX => part.X,
            TokenType.PartM => part.M,
            TokenType.PartA => part.A,
            TokenType.PartS => part.S,
            _ => throw new ArgumentOutOfRangeException()
        };

        return Compare(stack.Pop(), stack.Pop(), partValue);
    }
    
    private static bool Compare(Token compare, Token value, int var)
    {
        var toCompare = ToCompare(value);

        return compare.Type switch
        {
            TokenType.SmallerThen => var < toCompare,
            TokenType.BiggerThen => var > toCompare,
            _ => throw new InvalidOperationException("Unrecognized token")
        };
    }

    private static int ToCompare(Token value)
    {
        var conversionSuccess = int.TryParse(value.Value, out var toCompare);
        if (!conversionSuccess)
        {
            throw new InvalidCastException("value not a number: " + value);
        }

        return toCompare;
    }

    [Conditional("DEBUG")]
    private void Debug(string name)
    {
        Console.Write(name + " -> ");
    }
}