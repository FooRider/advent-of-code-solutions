using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("input1.txt");

{
    Console.WriteLine("Part 1");
    var matches = Regex.Matches(input, @"mul\(([0-9]{1,3}),([0-9]{1,3})\)");
    Console.WriteLine(matches.Sum(m => long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value)));
}

{
    Console.WriteLine("Part 2");
    var matches = Regex.Matches(input, @"((?<op>mul)\((?<operand1>[0-9]{1,3}),(?<operand2>[0-9]{1,3})\))|((?<op>do)\(\))|((?<op>don\'t)\(\))");
    var ops = matches.Select(m => (m.Groups["op"].Value, m.Groups["operand1"].Value, m.Groups["operand2"].Value));
    var enabled = true;
    long accumulator = 0;
    foreach (var (op, operand1, operand2) in ops)
    {
        if (op == "don't")
            enabled = false;
        if (op == "do")
            enabled = true;
        if (op == "mul" && enabled)
            accumulator += long.Parse(operand1) * long.Parse(operand2);
    }
    Console.WriteLine(accumulator);
}