namespace advent_19;

public class WorkFlowFacade(Dictionary<string, FlowFunction> functions, Part part)
{
    private const string StartFunction = "in";
    
    public bool Execute()
    {
        return functions[StartFunction].Execute(StartFunction, part, functions);
    }
}