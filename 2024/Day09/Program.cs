using Block = (int PositionOnDisk, int FileIdx, int PositionInFile, bool IsFile);

using var fh = File.OpenText("input1.txt");

var diskMapStr = await fh.ReadLineAsync();
var diskMap = diskMapStr!.Select(ch => (byte)(ch - '0')).ToArray();

{
    Console.WriteLine("Part 1");
    int diskPosition = 0;
    long checksum = 0; 
    foreach (var fileIdx in ReadDefragmentedFilesStep1())
        checksum += diskPosition++ * fileIdx;
    Console.WriteLine(checksum);
    
    IEnumerable<int> ReadDefragmentedFilesStep1()
    {
        using var fromStart = EnumerateFromStart().GetEnumerator();
        using var fromEnd = EnumerateFromEnd().GetEnumerator();
    
        fromStart.MoveNext();
        fromEnd.MoveNext();
    
        while (fromStart.Current.PositionOnDisk <= fromEnd.Current.PositionOnDisk)
        {
            if (!fromEnd.Current.IsFile)
            {
                fromEnd.MoveNext();
            }
            else if (fromStart.Current.IsFile)
            {
                yield return fromStart.Current.FileIdx;
                fromStart.MoveNext();
            }
            else if (fromEnd.Current.IsFile)
            {
                yield return fromEnd.Current.FileIdx;
                fromEnd.MoveNext();
                fromStart.MoveNext();
            }
        }
    }
    
    
    IEnumerable<Block> EnumerateFromStart()
    {
        bool isFile = false;
        int positionOnDisk = -1;
        int fileIdx = -1;
        foreach (var size in diskMap)
        {
            isFile = !isFile;
            int positionInFile = -1;
            if (isFile)
                fileIdx++;
            for (int i = 0; i < size; i++)
            {
                positionInFile++;
                positionOnDisk++;
                yield return (positionOnDisk, fileIdx, positionInFile, isFile);
            }
        }
    }
    
    IEnumerable<Block> EnumerateFromEnd()
    {
        int positionOnDisk = diskMap.Sum(s => s);
        int fileIdx = (diskMap.Length + 1) / 2;
        for (int j = diskMap.Length - 1; j >= 0; j--) 
        {
            int size = diskMap[j];
            var isFile = j % 2 == 0;
            int positionInFile = size;
            if (isFile)
                fileIdx--;
            for (int i = 0; i < size; i++)
            {
                positionInFile--;
                positionOnDisk--;
                yield return (positionOnDisk, fileIdx, positionInFile, isFile);
            }
        }
    }
}