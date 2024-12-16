namespace Day12;

public class Solver
{
    public static async Task<char[,]> LoadMapAsync(TextReader reader)
    {
        int? cols = null;
        List<char[]> lines = [];
        while (await reader.ReadLineAsync() is { } line)
        {
            cols ??= line.Length;
            lines.Add(line.ToCharArray());
        }
        var result = new char[cols!.Value, lines.Count];
        int row = 0;
        foreach (var line in lines)
        {
            for (int col = 0; col < line.Length; col++)
                result[col, row] = line[col];
            row++;
        }
        return result;
    }

    public static (char, bool[,]) ExtractRegion(char[,] map, bool[,] mask, int initX, int initY)
    {
        var regionMask = new bool[map.GetLength(0), map.GetLength(1)];
        
        var q = new Queue<(int X, int Y)>([(initX, initY)]);
        regionMask[initX, initY] = true;
        while (q.TryDequeue(out var coords))
        {
            var (x, y) = coords;
            foreach (var nCoords in AllNeighbors(map, x, y))
            {
                if (!mask[nCoords.X, nCoords.Y]) continue;
                if (regionMask[nCoords.X, nCoords.Y]) continue;
                if (map[x, y] != map[nCoords.X, nCoords.Y]) continue;
                q.Enqueue((nCoords.X, nCoords.Y));
                regionMask[nCoords.X, nCoords.Y] = true;
            }
        }
        return (map[initX, initY], regionMask);
    }

    public static IEnumerable<(char, bool[,])> ExtractAllRegions(char[,] map)
    {
        var mask = new bool[map.GetLength(0), map.GetLength(1)];
        foreach (var (x, y) in ForAllCoords(map))
            mask[x, y] = true;

        foreach (var (x, y) in ForAllCoords(map))
        {
            if (!mask[x, y]) continue;
            var region = ExtractRegion(map, mask, x, y);
            foreach (var (x1, y1) in ForAllCoords(region.Item2))
                if (region.Item2[x1, y1])
                    mask[x1, y1] = false;
            
            yield return region;
        }
    }

    public static (int Area, int Perimeter) MeasureRegion(bool[,] regionMask)
    {
        var area = 0;
        var perimeter = 0;
        foreach (var (x, y) in ForAllCoords(regionMask))
        {
            if (!regionMask[x, y]) continue;
            area++;
            var sameNeighbors = AllNeighbors(regionMask, x, y)
                .Count(n => regionMask[n.X, n.Y]);
            perimeter += 4 - sameNeighbors;
        }
        return (area, perimeter);
    }

    public static IEnumerable<FenceSegment> EnumerateFenceSegments(bool[,] regionMask)
    {
        var width = regionMask.GetLength(0);
        var height = regionMask.GetLength(1);
        foreach (var (x, y) in ForAllCoords(regionMask))
        {
            if (!regionMask[x, y]) continue;
            if (x == 0 || !regionMask[x - 1, y])
                yield return new FenceSegment(FenceLocation.West, x, y);
            if (y == 0 || !regionMask[x, y - 1])
                yield return new FenceSegment(FenceLocation.North, x, y);
            if (x == width - 1 || !regionMask[x + 1, y])
                yield return new FenceSegment(FenceLocation.East, x, y);
            if (y == height - 1 || !regionMask[x, y + 1])
                yield return new FenceSegment(FenceLocation.South, x, y);
        }
    }

    public static int CountFenceSides(bool[,] regionMask)
    {
        var t = EnumerateFenceSegments(regionMask)
            .GroupBy(fs => fs.Location)
            .ToDictionary(g => g.Key);

        int result = 0;

        foreach (var l in new[] { FenceLocation.West, FenceLocation.East })
        foreach (var row in t[l].GroupBy(fs => fs.X))
            result += GetContiguousIntervals(row.Select(fs => fs.Y));
        
        foreach (var l in new[] { FenceLocation.North, FenceLocation.South })
        foreach (var col in t[l].GroupBy(fs => fs.Y))
            result += GetContiguousIntervals(col.Select(fs => fs.X));
        
        return result;
    }

    public static int GetContiguousIntervals(IEnumerable<int> data)
    {
        var intervals = 0;
        int? last = null;
        foreach (var i in data.OrderBy(i => i))
        {
            if (i - 1 != last)
                intervals++;
            last = i;
        }

        return intervals;
    }

    public static long Part1(char[,] map)
    {
        long price = 0;
        foreach (var region in ExtractAllRegions(map))
        {
            var measurements = MeasureRegion(region.Item2);
            price += measurements.Area * measurements.Perimeter;
        }

        return price;
    }
    
    public static long Part2(char[,] map)
    {
        long price = 0;
        foreach (var region in ExtractAllRegions(map))
        {
            var measurements = MeasureRegion(region.Item2);
            var numSides = CountFenceSides(region.Item2);
            price += measurements.Area * numSides;
        }

        return price;
    }

    public static IEnumerable<(int X, int Y)> AllNeighbors<T>(T[,] map, int x, int y)
    {
        if (x > 0) yield return (x - 1, y);
        if (x < map.GetLength(1) - 1) yield return (x + 1, y);
        if (y > 0) yield return (x, y - 1);
        if (y < map.GetLength(1) - 1) yield return (x, y + 1);
    }

    public static IEnumerable<(int X, int Y)> ForAllCoords<T>(T[,] array2D)
    {
        int xDim = array2D.GetLength(0);
        int yDim = array2D.GetLength(1);
        for (int x = 0; x < xDim; x++)
        for (int y = 0; y < yDim; y++)
            yield return (x, y);
    }
}

public enum FenceLocation
{
    North, East, South, West
}
public record struct FenceSegment(FenceLocation Location, int X, int Y);