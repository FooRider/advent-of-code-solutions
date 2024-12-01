var inputFile = "input1.txt";

var list1 = new List<int>();
var list2 = new List<int>();

await using var fh = File.OpenRead(inputFile);
using var sr = new StreamReader(fh);
while (await sr.ReadLineAsync() is { } line)
{
    var ls = line.AsSpan();
    var len1 = ls.IndexOf(' ');
    list1.Add(int.Parse(line[..len1]));
    list2.Add(int.Parse(line[(len1 + 3)..]));
}

{
    Console.WriteLine("Part 1");
    list1.Sort();
    list2.Sort();
    Console.WriteLine(Enumerable.Zip(list1, list2, (a, b) => Math.Abs(a - b)).Sum());
}

{
    Console.WriteLine("Part 2");
    Console.WriteLine(CalculateSimilarity(list1, list2));

    long CalculateSimilarity(IEnumerable<int> sortedList1, IEnumerable<int> sortedList2)
    {
        using var sortedCounts1 = ReadCounts(sortedList1).GetEnumerator();
        using var sortedCounts2 = ReadCounts(sortedList2).GetEnumerator();

        long sum = 0;
        if (!sortedCounts1.MoveNext() || !sortedCounts2.MoveNext()) return sum;
        while (true)
        {
            if (sortedCounts1.Current.LocationId == sortedCounts2.Current.LocationId)
            {
                sum += sortedCounts1.Current.LocationId * sortedCounts1.Current.Count * sortedCounts2.Current.Count;
                if (!sortedCounts1.MoveNext()) return sum;
            }
            else if (sortedCounts1.Current.LocationId < sortedCounts2.Current.LocationId)
            {
                if (!sortedCounts1.MoveNext()) return sum;
            }
            else if (sortedCounts1.Current.LocationId > sortedCounts2.Current.LocationId)
            {
                if (!sortedCounts2.MoveNext()) return sum;
            }
        }
        
        
        IEnumerable<(int LocationId, int Count)> ReadCounts(IEnumerable<int> sortedLocationIds)
        {
            int currentCount = 0;
            int currentLocationId = 0;

            foreach (var locationId in sortedLocationIds)
            {
                if (locationId != currentLocationId)
                {
                    if (currentCount > 0)
                        yield return (currentLocationId, currentCount);
                
                    currentLocationId = locationId;
                    currentCount = 1;
                }
                else
                {
                    currentCount++;
                }
            }
            if (currentCount > 0)
                yield return (currentLocationId, currentCount);
        }
    }
}