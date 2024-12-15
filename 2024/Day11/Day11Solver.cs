namespace Day11;

public class Day11Solver
{
    private readonly Dictionary<(long, int), long> _memory = new();
    
    public long GetNumberOfStones(long stoneNumber, int numBlinks)
    {
        if (numBlinks == 0)
            return 1;
        
        if (_memory.ContainsKey((stoneNumber, numBlinks)))
            return _memory[(stoneNumber, numBlinks)];
        
        long result;
        if (stoneNumber == 0)
        {
            result = GetNumberOfStones(1, numBlinks - 1);
        }
        else
        {
            var numDigits = NumberOfDigits(stoneNumber);
            if (numDigits % 2 == 0)
            {
                var div = TenToX(numDigits / 2);
                var n1 = stoneNumber / div;
                var n2 = stoneNumber % div;
                result = GetNumberOfStones(n1, numBlinks - 1) + GetNumberOfStones(n2, numBlinks - 1);
            }
            else
            {
                result = GetNumberOfStones(stoneNumber * 2024, numBlinks - 1);
            }
        }
        _memory[(stoneNumber, numBlinks)] = result;
        return result;
    }

    public static int NumberOfDigits(long number)
    {
        if (number < 10) return 1;
        int numDigits = 0;
        while (number > 0)
        {
            number /= 10;
            numDigits++;
        }
        return numDigits;
    }

    public static long TenToX(int x)
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