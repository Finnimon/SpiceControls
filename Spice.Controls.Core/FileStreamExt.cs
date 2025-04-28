using System.Runtime.CompilerServices;
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
            if (read != chunkSize)
                throw new ArgumentOutOfRangeException(nameof(chunkCount),
                    $"TotalChunkCount=={i}\r\nMissing {chunkCount - i} chunks");
            yield return chunk;
        }
    }

    public static IEnumerable<byte[]> ReadChunky(this FileStream me, FilePosition position, int chunkSize)
    {
        me.Seek(position);
        var count = position.Length / chunkSize;
        return me.ReadChunky(chunkSize, count);
    }
    
    private sealed class ReadUntilFoundHelper(FileStream reader,bool littleEndian,bool utf16)
    {
        private byte[] Buffer { get; } = [0, 0];
        private readonly int _firstPos = littleEndian ? 0 : 1;
        private readonly int _secondPos = littleEndian ? 1 : 0;

        public byte First
        {
            get => Buffer[_firstPos];
            set => Buffer[_firstPos] = value;
        }

        public byte Second
        {
            get => Buffer[_secondPos];
            set => Buffer[_secondPos] = value;
        }
        public char? GetNextChar()
        {
            var read = reader.ReadByte();
            var fail = read == -1;
            if(fail) return null;    
            First = (byte) read;
            if(utf16) Second = (byte)reader.ReadByte();
            return BitConverter.ToChar(Buffer);
        }
    }

    public static string ReadUntilFound(this FileStream me, string match, bool isUtf16, bool littleEndian,
        int matchCount = 1)
    {
        if (matchCount < 1) throw new ArgumentOutOfRangeException(nameof(matchCount), matchCount, "Must be >1");
        var matchLength = match.Length;
        var matchArray = match.ToCharArray();
        if (matchLength == 0)
            throw new ArgumentOutOfRangeException(nameof(match), $"Length==0\r\nMissing {matchLength} chunks");
        var matchTargetCount = matchCount;
        List<char> result = [];
        var helper = new ReadUntilFoundHelper(me,littleEndian,isUtf16);
        while (matchCount > 0)
        {
            var read = helper.GetNextChar();
            if (read is null) break;
            
            result.Add(read.Value);
            var resultCount = result.Count;
            if (resultCount < matchLength) continue;

            var found = result.EndsWith(matchArray);
            if (found) matchCount--;
        }

        if (matchCount != 0)
            throw new ArgumentException(
                $"{match} not found\nFound total:{matchTargetCount - matchCount}\nMissing: {matchCount}");

        return new string([..result]);
    }
}