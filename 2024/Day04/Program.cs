var lines = await File.ReadAllLinesAsync("input1.txt");
var width = lines[0].Length;
var height = lines.Length;

{
    Console.WriteLine("Part 1");

    int xmasCount = 0;
    
    for (int y = 0; y < height; y++)
    for (int x = 0; x < width; x++)
    foreach (var (directionX, directionY) in EnumerateDirections())
    {
        if (ReadString(lines, y, x, directionX, directionY, 4) == "XMAS")
            xmasCount++;
    }
    Console.WriteLine(xmasCount);

    IEnumerable<(int, int)> EnumerateDirections()
    {
        yield return (1, 0);
        yield return (1, 1);
        yield return (0, 1);
        yield return (-1, 1);
        yield return (-1, 0);
        yield return (-1, -1);
        yield return (0, -1);
        yield return (1, -1);
    }
}

{
    Console.WriteLine("Part 2");
    
    int xmasCount = 0;
    
    for (int y = 0; y < height; y++)
    for (int x = 0; x < width; x++)
    {
        var diag1 = ReadString(lines, y - 1, x - 1, 1, 1, 3);
        var diag2 = ReadString(lines, y - 1, x + 1, -1, 1, 3);
        if ((diag1 == "MAS" || diag1 == "SAM") && (diag2 == "MAS" || diag2 == "SAM"))
            xmasCount++;
    }
    Console.WriteLine(xmasCount);
}

string ReadString(string[] lines, int startRow, int startCol, int directionX, int directionY, int length)
{
    if (startRow < 0 || startRow >= width || startCol < 0 || startCol >= height || length <= 0)
        return string.Empty;
    return
        $"{lines[startRow][startCol]}{ReadString(lines, startRow + directionY, startCol + directionX, directionX, directionY, length - 1)}";
}