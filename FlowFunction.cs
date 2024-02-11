using System.Diagnostics;
using advent_19.Tokenizer;

namespace advent_19;

public class FlowFunction(Stack<(TokenType, string)> tokenStack)
{
    public bool Execute(string name, Part p, Dictionary<string, FlowFunction> functions)
    {
        Debug(name);
        var clonedStack = new Stack<(TokenType, string)>(new Stack<(TokenType, string)>(tokenStack));
        
        while (clonedStack.Count > 0)
        {
            var token = clonedStack.Pop();
            switch (token.Item1)
            {
                case TokenType.PartS:
                case TokenType.PartM:
                case TokenType.PartA:
                case TokenType.PartX:
                    var success = ExecutePart(token.Item1, p, clonedStack);
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
                    if (tokenAhead.Item1 == TokenType.Function)
                    {
                        var function = clonedStack.Pop();
                        return functions[function.Item2].Execute(function.Item2, p, functions);
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
        
        var clonedStack = new Stack<(TokenType, string)>(new Stack<(TokenType, string)>(tokenStack));
        var values = new List<(TokenType, int)>();

        while (clonedStack.Count > 0)
        {
            var token = clonedStack.Pop();

            if (token.Item1 == type)
            {
                var compareToken = clonedStack.Pop();
                var numberToken = clonedStack.Pop();
                var toCompare = ToCompare(numberToken);

                values.Add((compareToken.Item1, toCompare));
            }
        }

        return values;
    }

    public string GetFunctionName()
    {
        return tokenStack.First().Item2;
    }

    private static bool ExecutePart(TokenType tokenType, Part part, Stack<(TokenType, string)> stack)
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
    
    private static bool Compare((TokenType, string) compare, (TokenType, string) value, int var)
    {
        var toCompare = ToCompare(value);

        return compare.Item1 switch
        {
            TokenType.SmallerThen => var < toCompare,
            TokenType.BiggerThen => var > toCompare,
            _ => throw new InvalidOperationException("Unrecognized token")
        };
    }

    private static int ToCompare((TokenType, string) value)
    {
        var conversionSuccess = int.TryParse(value.Item2, out var toCompare);
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