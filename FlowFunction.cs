using System.Diagnostics;

namespace advent_19;

public class FlowFunction
{
    private readonly string _rule;
    private readonly string[] _parts;

    public FlowFunction(string rule)
    {
        _rule = rule;
        var functionName = GetFunctionName();
        var contentInsideBraces = rule.Substring(functionName.Length + 1, rule.Length-functionName.Length - 2);
        _parts = contentInsideBraces.Split(",");
    }

    public bool Execute(string name, Part p, Dictionary<string, FlowFunction> functions)
    {
        Debug(name);
        foreach (var subpart in _parts)
        {
            // if a subpart is true we execute the part after :
            if (subpart.IndexOf(':') > 0)
            {
                var colonIndex = subpart.IndexOf(':');
                var cond = subpart[..colonIndex];
                var afterCond = subpart[(colonIndex + 1)..];

                if (EvalCond(cond, p))
                {
                    return ExecuteCond(afterCond, p, functions);
                }
                continue;
            }

            return ExecuteCond(subpart, p, functions);
        }
        return false;
    }

    private static bool ExecuteCond(string cond, Part p, Dictionary<string, FlowFunction> functions)
    {
        if (cond == "A")
        {
            return true;
        } 
        if (cond == "R")
        {
            return false;
        }
                    
        if (functions.TryGetValue(cond, out var function))
        {
            return function.Execute(cond, p, functions);
        }

        return false;
    }
    
    private static bool EvalCond(string cond, Part p)
    {
        var operators = new List<string> { ">", "<" };
        var operatorIndex = operators.FindIndex(cond.Contains);
        var operatorSymbol = operators[operatorIndex];
        var parts = cond.Split(operatorSymbol);
        var partName = parts[0];
        var number = Convert.ToInt32(parts[1]);

        var value = partName switch
        {
            "x" => p.X,
            "m" => p.M,
            "a" => p.A,
            "s" => p.S,
            _ => throw new ArgumentOutOfRangeException()
        };

        return operatorSymbol switch
        {
            "<" => value < number,
            ">" => value > number,
            _ => throw new ArgumentOutOfRangeException()
        };

    }
    
    public string GetFunctionName()
    {
        return _rule.Split("{")[0];
    }

    [Conditional("DEBUG")]
    private void Debug(string name)
    {
        Console.Write(name + " -> ");
    }
}