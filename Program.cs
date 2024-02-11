using advent_19;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

//const string inputFile = "configurations/example.flow";
const string inputFile = "configurations/input.flow";
var (rules, parts) = FlowFileParser.Parse(inputFile);

var functions = new Dictionary<string, FlowFunction>();
foreach (var rule in rules)
{
    var function = new FlowFunction(rule);
    functions.Add(function.GetFunctionName(), function);
}

var sum = 0;
foreach (var part in parts)
{
#if DEBUG
    Console.Write(part + ": ");
#endif
    var workflow = new WorkFlowFacade(functions, part);
    var result = workflow.Execute();
    if (result)
    {
        sum += part.Sum();
    }
#if DEBUG
    Console.WriteLine(result ? "A" : "R");
#endif
}

Console.WriteLine($"Result: {sum}");

watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
