using System.Text;

namespace Spice.Controls.Core;

public static class FileStreamExt
{
    public static long Seek(this FileStream me, FilePosition position)
        => me.Seek(position.Offset, position.Origin);

    public static byte[] ReadExactly(this FileStream fileStream, FilePosition position, byte[]? buffer = null)
    {
        buffer ??= new byte[position.Length];
        fileStream.Seek(position);
        fileStream.ReadExactly(buffer);
        return buffer;
    }

    public static IEnumerable<byte[]> ReadChunky(this FileStream me, int chunkSize)
    {
        var chunk = new byte[chunkSize];
        while (true)
        {
            var read = me.Read(chunk, 0, chunkSize);
            if (read != chunkSize) yield break;
            yield return chunk;
        }
    }

    public static IEnumerable<byte[]> ReadChunky(this FileStream me, int chunkSize, long chunkCount)
    {
        var chunk = new byte[chunkSize];
        for (long i = 0; i < chunkCount; i++)
        {
            var read = me.Read(chunk, 0, chunkSize);
            if (read != chunkSize) throw new ArgumentOutOfRangeException(nameof(chunkCount),$"TotalChunkCount=={i}\r\nMissing {chunkCount-i} chunks");
            yield return chunk;
        }
    }

    public static IEnumerable<byte[]> ReadChunky(this FileStream me, FilePosition position, int chunkSize)
    {
        me.Seek(position);
        var count=position.Length/chunkSize;
        return me.ReadChunky(chunkSize, count);
    }

    public static string ReadUntilFound(this FileStream me, string match, bool isUtf16, int matchCount = 1)
    {
        if (matchCount < 1) throw new ArgumentOutOfRangeException(nameof(matchCount), matchCount, "Must be >1");
        var matchLength = match.Length;
        if(matchLength==0) throw new ArgumentOutOfRangeException(nameof(match),$"Length==0\r\nMissing {matchLength} chunks");
        var matchTargetCount = matchCount;   
        List<char> result = [];
        while (matchCount>0)
        {
            var readInt = me.ReadByte();
            if (readInt==-1) break;
            char read;
            if (isUtf16)
            {
                var second = me.ReadByte();
                if (second==-1) break;
                read=BitConverter.ToChar([(byte)readInt, (byte)second]);
            }
            else
            {
                read=(char) readInt;
            }
            result.Add(read);
            var resultCount = result.Count;
            if(resultCount<matchLength) continue;
            
            var tail = result.Slice(resultCount - matchLength, matchLength);
            var found = tail.SequenceEqual(match);
            if (found) matchCount--;
        }

        if (matchCount!=0) throw new ArgumentException($"{match} not found\nFound total:{matchTargetCount-matchCount}\nMissing: {matchCount}");
        
        return new StringBuilder().AppendJoin(string.Empty,result).ToString();
    }
}