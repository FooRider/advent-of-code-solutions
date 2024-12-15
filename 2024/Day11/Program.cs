List<long> input = [];

{
    using var fh = File.OpenText("input1.txt");
    var ls = (await fh.ReadLineAsync()).AsSpan();
    var lsEnum = ls.Split(' ');
    while (lsEnum.MoveNext())
        input.Add(long.Parse(ls[lsEnum.Current]));
}

var solver = new Day11.Day11Solver();

{
    Console.WriteLine("Part 1");
    var result1 = input.Sum(num => solver.GetNumberOfStones(num, 25));
    Console.WriteLine(result1);
}

{
    Console.WriteLine("Part 2");
    var result2 = input.Sum(num => solver.GetNumberOfStones(num, 75));
    Console.WriteLine(result2);
}