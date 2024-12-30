namespace Day22;

public class Solver
{
    public static long Mix(long number1, long number2) => number1 ^ number2;
    public static long Prune(long number1) => number1 % 16_777_216;

    public static long Iterate(long secretNumber)
    {
        var a = secretNumber << 6;
        secretNumber = Prune(Mix(a, secretNumber));
        var b = secretNumber >> 5;
        secretNumber = Prune(Mix(b, secretNumber));
        var c = secretNumber * 2048;
        secretNumber = Prune(Mix(c, secretNumber));
        return secretNumber;
    }

    public static long Iterate(long secretNumber, long numIterations)
    {
        for (long i = 0; i < numIterations; i++)
            secretNumber = Iterate(secretNumber);
        return secretNumber;
    }

    public static long Part1(IReadOnlyCollection<long> input)
    {
        long result = 0;
        foreach (var sn in input)
        {
            result += Iterate(sn, 2000);
        }

        return result;
    }

    public static IEnumerable<(int Change, int Price)> CalculatePricesAndChanges(long initialSecretNumber, int numIterations)
    {
        var sn = initialSecretNumber;
        var price = (int)(sn % 10);
        for (int i = 0; i < numIterations; i++)
        {
            sn = Solver.Iterate(sn);
            var nextPrice = (int)(sn % 10);
            yield return (Change: nextPrice - price, Price : nextPrice);
            price = nextPrice;
        }
    }

    public static IEnumerable<(int Ch1, int Ch2, int Ch3, int Ch4, int Price)>
        PrecalculatePart2Input(IEnumerable<(int Change, int Price)> prices)
    {
        var usedChanges = new HashSet<(int, int, int, int)>();
        
        using var e = prices.GetEnumerator();
        e.MoveNext();
        var (ch1, _) = e.Current;
        e.MoveNext();
        var (ch2, _) = e.Current;
        e.MoveNext();
        var (ch3, _) = e.Current;
        while (e.MoveNext())
        {
            var (ch4, price) = e.Current;

            if (!usedChanges.Contains((ch1, ch2, ch3, ch4)))
            {
                yield return (ch1, ch2, ch3, ch4, price);
                usedChanges.Add((ch1, ch2, ch3, ch4));
            }

            ch1 = ch2;
            ch2 = ch3;
            ch3 = ch4;
        }
    }

    public static long SolvePart2(IReadOnlyCollection<long> input)
    {
        var precalculated = new List<Dictionary<(int, int, int, int), int>>();
        foreach (var sn in input)
        {
            precalculated.Add(PrecalculatePart2Input(CalculatePricesAndChanges(sn, 2000))
                .ToDictionary(i => (i.Ch1, i.Ch2, i.Ch3, i.Ch4), i => i.Price));
        }

        var allChanges = precalculated.SelectMany(pc => pc.Keys).Distinct().ToList();
        long bestPrice = -1;
        foreach (var ch in allChanges)
        {
            long acc = 0;
            foreach (var pc in precalculated)
            {
                if (pc.ContainsKey(ch))
                    acc += pc[ch];
            }
            if (acc > bestPrice)
                bestPrice = acc;
        }

        return bestPrice;
    }
}