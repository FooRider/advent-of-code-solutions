var inputFile = "input1.txt";

await using var fh = File.OpenRead(inputFile);
using var sr = new StreamReader(fh);

int safeCount = 0;
while (await sr.ReadLineAsync() is { } line)
{
    bool isUnsafe = false;
    var ls = line.AsSpan();
    var enumerator = ls.Split(' ');
    if (!enumerator.MoveNext()) continue;
    var prev1 = int.Parse(ls[enumerator.Current]);
    if (!enumerator.MoveNext()) continue;
    var prev0 = int.Parse(ls[enumerator.Current]);
    
    var direction = Math.Sign(prev0 - prev1);
    if (Math.Abs(prev0 - prev1) is < 1 or > 3)
    {
        isUnsafe = true;
    }
    
    while (!isUnsafe && enumerator.MoveNext())
    {
        prev1 = prev0;
        prev0 = int.Parse(ls[enumerator.Current]);
        if (Math.Abs(prev0 - prev1) is < 1 or > 3 || Math.Sign(prev0 - prev1) != direction)
        {
            isUnsafe = true;
        };
    }
    
    if (!isUnsafe)
        safeCount++;
}

Console.WriteLine(safeCount);