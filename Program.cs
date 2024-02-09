using advent_19;
using advent_19.Tokenizer;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

// const string inputFile = "configurations/example.flow";
const string inputFile = "configurations/input.flow";
var (tokensList, parts) = FlowFileParser.Parse(inputFile);

var functions = new Dictionary<string, FlowFunction>();
foreach (var tokens in tokensList)
{
    var function = new FlowFunction(tokens);
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

void GetNumbers(TokenType tokenType)
{
    var numbers = new List<int>();
    foreach (var tokens in functions.Values.ToList().Select(function => function.FindValues(tokenType)))
    {
        foreach (var (_, value) in tokens)
        {
            numbers.Add(value);
        }
    }
    numbers.Sort();
    Console.WriteLine($"Range of {tokenType}: {numbers[0]} -> {numbers[^1]}");
}

// Part 2
GetNumbers(TokenType.PartX);
GetNumbers(TokenType.PartM);
GetNumbers(TokenType.PartA);
GetNumbers(TokenType.PartS);

watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
