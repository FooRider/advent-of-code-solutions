const string input = "input1.txt";

long part1Sum = 0;
long part2Sum = 0;

using var fh = File.OpenText(input);
while (await fh.ReadLineAsync() is { } line)
{
    var ls = line.AsSpan();
    var enumerator = ls.Split(' ');
    enumerator.MoveNext();
    var result = long.Parse(ls[0..(enumerator.Current.End.Value - 1)]);
    var operands = new List<long>();
    while (enumerator.MoveNext())
        operands.Add(long.Parse(ls[enumerator.Current]));

    if (CanBeMadeTrue(result, operands, false)) part1Sum += result;
    if (CanBeMadeTrue(result, operands, true)) part2Sum += result;
}

Console.WriteLine("Part 1");
Console.WriteLine(part1Sum);

Console.WriteLine("Part 2");
Console.WriteLine(part2Sum);

bool CanBeMadeTrue(long result, IReadOnlyCollection<long> operands, bool allowConcatenation)
{
    return CanBeMadeTrue(result, 0, '+', operands.ToArray(), allowConcatenation);
    
    bool CanBeMadeTrue(long result, long accumulator, char operator1, Span<long> operands, bool allowConcatenation)
    {
        if (operands.IsEmpty)
            return result == accumulator;

        var nextAccumulator = operator1 switch
        {
            '+' => accumulator + operands[0],
            '*' => accumulator * operands[0],
            '|' => long.Parse(accumulator.ToString() + operands[0].ToString()),
            _ => throw new Exception($"Unexpected operator {operator1}")
        };

        return CanBeMadeTrue(result, nextAccumulator, '+', operands.Slice(1), allowConcatenation)
            || CanBeMadeTrue(result, nextAccumulator, '*', operands.Slice(1), allowConcatenation)
            || (allowConcatenation && CanBeMadeTrue(result, nextAccumulator, '|', operands.Slice(1), allowConcatenation));
    }
}