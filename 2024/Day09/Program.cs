using Block = (int PositionOnDisk, int FileIdx, int PositionInFile, bool IsFile);
using ContiguousFile = (int PositionOnDisk, int Size, int FileIdx);
using ContiguousFreeSpace = (int PositionOnDisk, int Size);

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

{
    Console.WriteLine("Part 2");

    var fileMap = new List<ContiguousFile>();
    var freeSpaceMap = new List<ContiguousFreeSpace>();

    bool isFile = false;
    int fileIdx = 0;
    int positionOnDisk = 0;
    foreach (var size in diskMap)
    {
        isFile = !isFile;
        if (isFile)
            fileMap.Add((positionOnDisk, size, fileIdx++));
        else
            freeSpaceMap.Add((positionOnDisk, size));
        positionOnDisk += size;
    }

    var allFilesFromEnd = fileMap.AsEnumerable().Reverse().ToArray();
    foreach (var file in allFilesFromEnd)
    {
        var targetPosition = freeSpaceMap
            .Cast<ContiguousFreeSpace?>()
            .Select(fs => fs)
            .FirstOrDefault(fs => fs.HasValue && fs.Value.Size >= file.Size);

        if (targetPosition.HasValue && targetPosition.Value.PositionOnDisk < file.PositionOnDisk)
        {
            fileMap.Remove(file);
            fileMap.Add((targetPosition.Value.PositionOnDisk, file.Size, file.FileIdx));
            var fsIdx = freeSpaceMap.FindIndex(fs => fs.PositionOnDisk == targetPosition.Value.PositionOnDisk);
            freeSpaceMap[fsIdx] = (targetPosition.Value.PositionOnDisk + file.Size, targetPosition.Value.Size - file.Size);
        }
    }

    ContiguousFile prevFile = (0, 0, 0);
    long checksum = 0;
    foreach (var file in fileMap.OrderBy(fm => fm.PositionOnDisk))
    {
        for (int i = prevFile.PositionOnDisk + prevFile.Size; i < file.PositionOnDisk; i++)
        {
            //Console.Write('.');
        }
        for (int i = 0; i < file.Size; i++)
        {
            checksum += (file.PositionOnDisk + i) * file.FileIdx;
            //Console.Write(file.FileIdx);
        }
        prevFile = file;
    }
    Console.WriteLine(checksum);
}

