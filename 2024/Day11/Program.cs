List<long> input = [];

{
    using var fh = File.OpenText("input0a.txt");
    var ls = (await fh.ReadLineAsync()).AsSpan();
    var lsEnum = ls.Split(' ');
    while (lsEnum.MoveNext())
        input.Add(long.Parse(ls[lsEnum.Current]));
}


{
    Console.WriteLine("Part 1");
    var result = input.Sum(num => GetNumberOfStones(num, 25));
    Console.WriteLine(result);
    // 203586 too low

    long GetNumberOfStones(long stoneNumber, int numBlinks)
    {
        if (numBlinks == 0) return 1;
        if (stoneNumber == 0) return GetNumberOfStones(1, numBlinks - 1);
        var numDigits = NumberOfDigits(stoneNumber);
        if (numDigits % 2 == 0)
        {
            var div = TenToX(numDigits / 2);
            var n1 = stoneNumber / div;
            var n2 = stoneNumber % div;
            return GetNumberOfStones(n1, numBlinks - 1) + GetNumberOfStones(n2, numBlinks - 1);
        }
        return GetNumberOfStones(stoneNumber * 2024, numBlinks - 1);
    }

    int NumberOfDigits(long number)
    {
        if (number <= 10) return 1;
        int numDigits = 0;
        while (number > 0)
        {
            number /= 10;
            numDigits++;
        }
        return numDigits;
    }

    long TenToX(int x)
    {
        long result = 1;
        while (x > 0)
        {
            result *= 10;
            x--;
        }
        return result;
    }
}