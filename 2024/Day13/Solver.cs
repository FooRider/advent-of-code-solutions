using MathNet.Numerics.LinearAlgebra;

namespace Day13;

public class Solver
{
    public static int SolveClawMachinePart1(ClawMachine cm)
    {
        for (int a = 0; a <= 100; a++)
        for (int b = 0; b <= 100; b++)
        {
            if (cm.A[0] * a + cm.B[0] * b == cm.Prize[0]
                && cm.A[1] * a + cm.B[1] * b == cm.Prize[1])
            {
                return a * 3 + b * 1;
            }
        }
        return 0;
    }

    public static long SolveClawMachinePart2(ClawMachine cm)
        => SolveClawMachineNoLimits(cm with
        {
            Prize = cm.Prize.Select(v => v + 10000000000000).ToArray()
        });

    public static long SolveClawMachineNoLimits(ClawMachine cm)
    {
        var va = Vector<double>.Build.DenseOfArray(cm.A.Select(v => (double)v).ToArray());
        var vb = Vector<double>.Build.DenseOfArray(cm.B.Select(v => (double)v).ToArray());
        var vprize = Vector<double>.Build.DenseOfArray(cm.Prize.Select(v => (double)v).ToArray());
        var mat = Matrix<double>.Build.DenseOfColumns([va, vb]);

        if (!(mat.Solve(vprize) is { } solution))
            return 0;

        var a0 = (long)solution[0];
        var b0 = (long)solution[1];

        for (int i = 0; i < 2; i++)
        for (int j = 0; j < 2; j++)
        {
            long a = a0 + i;
            long b = b0 + j;
            if (a * cm.A[0] + b * cm.B[0] == cm.Prize[0] && a * cm.A[1] + b * cm.B[1] == cm.Prize[1])
            {
                return a * 3 + b * 1;
            }
        }

        return 0;
    }

    public static async Task<ClawMachine?> TryLoadAsync(TextReader input)
    {
        var line = await input.ReadLineAsync();
        if (line is null) return null;
        
        if (!line.StartsWith("Button A: X")) throw new Exception();
        var a = ParseVector(line.AsSpan("Button A: ".Length));
        
        line = await input.ReadLineAsync();
        if (!line.StartsWith("Button B: X")) throw new Exception();
        var b = ParseVector(line.AsSpan("Button B: ".Length));
        
        line = await input.ReadLineAsync();
        if (!line.StartsWith("Prize: X=")) throw new Exception();
        var prize = ParseVector(line.AsSpan("Prize: ".Length));

        line = await input.ReadLineAsync();
        if (!string.IsNullOrEmpty(line)) throw new Exception();
        
        return new ClawMachine(a, b, prize);
    }

    public static bool AreLinearlyDependent(long[] a, long[] b)
    {
        var an = SimplifyFraction(a[0], a[1]);
        var bn = SimplifyFraction(b[0], b[1]);
        return an == bn;
    }

    public static (long, long) SimplifyFraction(long a1, long a2)
    {
        var gcd = GCD(a1, a2);
        if (gcd <= 1) return (a1, a2);
        return (a1 / gcd, a2 / gcd);
    }

    public static long GCD(long a, long b)
    {
        if (a == b) return a;
        if (a < b) return GCD(b, a);
        if (b < 0) return GCD(a, -b);
        if (a < 0) return GCD(-a, b);
        if (b == 0) return a;
        return GCD(b, a % b);
    }

    private static long[] ParseVector(ReadOnlySpan<char> input)
    {
        var xPos = input.IndexOf('X');
        var delimPos = input.IndexOf(',');
        var yPos = input.IndexOf('Y');

        var x = ReadLong(input[(xPos + 1)..(delimPos)]);
        var y = ReadLong(input[(yPos + 1)..]);
        return [x, y];
    }

    private static long ReadLong(ReadOnlySpan<char> input)
    {
        if (input[0] == '=')
            input = input.Slice(1);
        return long.Parse(input);
    }
}

public record struct ClawMachine(long[] A, long[] B, long[] Prize);