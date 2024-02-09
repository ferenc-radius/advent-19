using advent_19;
var watch = new System.Diagnostics.Stopwatch();
            
watch.Start();

//const string inputFile = "configurations/example.flow";
const string inputFile = "configurations/input.flow";
var fileParser = new FlowFileParser(inputFile);
Console.WriteLine("available functions: " + fileParser.Functions.Count);

var sum = 0;
foreach (var part in fileParser.Parts)
{
    Console.Write(part + ": ");
    var workflow = new WorkFlowFacade(fileParser.Functions, part);
    var result = workflow.Execute();
    if (result)
    {
        sum += part.Sum();
    }
    
    Console.WriteLine(result ? "A" : "R");
}

Console.WriteLine("Result: " + sum);
watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
