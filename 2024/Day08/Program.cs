using Point = (int X, int Y);

using var fh = File.OpenText("input1.txt");
int y = 0;
var antennae = new Dictionary<char, List<Antenna>>();
int width = -1;
while (await fh.ReadLineAsync() is { } line)
{
    width = line.Length;
    foreach (var antenna in
             line.Select((ch, idx) => (ch, idx))
                 .Where(t => Antenna.IsAntenna(t.ch))
                 .Select(t => new Antenna(t.idx, y, t.ch)))
    {
        if (!antennae.ContainsKey(antenna.Frequency))
            antennae.Add(antenna.Frequency, []);
        antennae[antenna.Frequency].Add(antenna);
    }
    y++;
}
int height = y;

{
    Console.WriteLine("Part 1");
    List<Point> antinodes = [];
    foreach (var (freq, ants) in antennae)
    {
        foreach (var (a1, a2) in GeneratePairs(ants))
        {
            Point vectorA12 = (a2.X - a1.X, a2.Y - a1.Y);
            List<Point> ans = [(a2.X + vectorA12.X, a2.Y + vectorA12.Y), (a1.X - vectorA12.X, a1.Y - vectorA12.Y)];
            foreach (var point in ans
                         .Where(p => p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height)
                         .Where(p => !antinodes.Contains(p)))
                antinodes.Add(point);
        }
    }
    Console.WriteLine(antinodes.Count);
}

{
    Console.WriteLine("Part 2");
    List<Point> antinodes = [];
    foreach (var (freq, ants) in antennae)
    {
        foreach (var (a1, a2) in GeneratePairs(ants))
        {
            Point vectorA12 = Normalize((a2.X - a1.X, a2.Y - a1.Y));
            var ans = EnumerateToInfinity()
                .Select<int, Point>(x => (a1.X + vectorA12.X * x, a1.Y + vectorA12.Y * x))
                .TakeWhile(p => p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height)
                .Concat(EnumerateToInfinity()
                    .Select<int, Point>(x => (a1.X - vectorA12.X * x, a1.Y - vectorA12.Y * x))
                    .TakeWhile(p => p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < height));
            
            foreach (var point in ans.Where(p => !antinodes.Contains(p)))
                antinodes.Add(point);
        }
    }
    Console.WriteLine(antinodes.Count);

    Point Normalize(Point vector)
    {
        var gcd = GCD(Math.Abs(vector.X), Math.Abs(vector.Y));
        return new Point(vector.X / gcd, vector.Y / gcd);

        int GCD(int x, int y)
        {
            if (y > x) return GCD(y, x);
            if (x == y || x % y == 0) return y;
            return GCD(y, x % y);
        }
    }

    IEnumerable<int> EnumerateToInfinity()
    {
        int i = 0;
        while (true) yield return i++;
    }
}

IEnumerable<(T, T)> GeneratePairs<T>(List<T> input)
{
    for (var i = 0; i < input.Count - 1; i++)
    for (var j = i + 1; j < input.Count; j++)
        yield return (input[i], input[j]);
}

readonly record struct Antenna(int X, int Y, char Frequency)
{
    public static bool IsAntenna(char ch) => char.IsLetterOrDigit(ch);
}