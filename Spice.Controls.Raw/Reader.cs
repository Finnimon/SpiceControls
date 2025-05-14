using System.Globalization;
using Spice.Controls.Core;

namespace Spice.Controls.Raw;

public static class Reader
{
    #region Read SpiceRaw

    public static SpiceRaw ReadSpiceRaw(string file)
    {
        var raw=File.OpenRead(file);
        var header=ReadHeader(raw);
        var (time, measurements) = ReadBody(raw, header);
        return new SpiceRaw(header,time,measurements);
    }

    #endregion
    #region Read Header
   
    public static SpiceRawHeader ReadHeader(FileStream file)
    {
        file.Seek(0, SeekOrigin.Begin);
        var firstIsZero = file.ReadByte() == 0;
        var secondIsZero = file.ReadByte() == 0;
        var isUtf16 = firstIsZero || secondIsZero;
        file.Seek(0, SeekOrigin.Begin);
        var headerStr= file.ReadUntilFound("Binary:\n",isUtf16,secondIsZero);
        var binaryDataOffset = headerStr.Length;
        if (isUtf16) binaryDataOffset *= 2;
        var binaryPosition = new FilePosition(binaryDataOffset, file.Length-binaryDataOffset);
        headerStr = headerStr.Replace('\t', ' ');
        var lines=headerStr.Split('\n',StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        
        const string dateTimeFormat = "ddd MMM dd HH:mm:ss yyyy";
        
        var title = lines[0]["Title: * ".Length..];
        var date = DateTime.ParseExact(ExtractHeaderLineValue(lines, "Date:"),format:dateTimeFormat,CultureInfo.InvariantCulture);
        var plotname=ExtractHeaderLineValue(lines,"Plotname:");
        var flags=ExtractHeaderLineValue(lines, "Flags:").Split(' ');
        var noPoints = int.Parse(ExtractHeaderLineValue(lines,"No. Points:"));
        var noVars=int.Parse(ExtractHeaderLineValue(lines,"No. Variables:"));
        var offset=double.Parse(ExtractHeaderLineValue(lines,"Offset:"));
        var command=ExtractHeaderLineValue(lines,"Command:");
        
        var measurements = ExtractVariables(lines, noVars);

        var variables = measurements.ToArray();
        return new SpiceRawHeader(title,date,plotname,flags,noVars,noPoints,offset,command,variables,binaryPosition,headerStr);
    }

    private static List<SpiceMeasurement> ExtractVariables(string[] lines, int noVars)
    {
        var firstVarIndex=lines.TakeWhile(x=>!x.StartsWith("Variables:")).Count()+2;
        List<SpiceMeasurement> measurements = [new("time",Unit.NanoSecond)];
        noVars--;
        for (var i = firstVarIndex; i < noVars + firstVarIndex; i++)
        {
            var line = lines[i];
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var relevant = split[1];
            var unit = relevant[0] switch
            {
                'V' => Unit.Voltage,
                'I' => Unit.Ampere,
                _ => throw new NotSupportedException($"ElectricUnit {relevant[0]} not implemented."),
            };
            var name=relevant.Substring(2,relevant.Length-3);
            measurements.Add(new SpiceMeasurement(name,unit));
        }

        return measurements;
    }

    private static string ExtractHeaderLineValue(IEnumerable<string> headerLines, string startsWith) 
        => headerLines.First(x => x.StartsWith(startsWith))[startsWith.Length..].Trim();

    #endregion
    #region Read Body

    public static (ulong[] time, Dictionary<string, float[]> measurements) ReadBody(FileStream raw, SpiceRawHeader header)
    {
        var (time, measurementValues) = ReadBinary(raw, header);
        Dictionary<string, float[]> measurements = [];
        for (var i = 1; i < header.Variables.Length; i++)
        {
            var id = header.Variables[i].Id;
            measurements[id] = measurementValues[i-1];
        }
        return (time, measurements);
    }
    
    private static (ulong[] time, List<float[]> measurements) ReadBinary(FileStream raw, SpiceRawHeader header)
    {
        var numberOfFloats = header.Variables.Length - 1;
        var chunkSize = sizeof(float) * numberOfFloats + sizeof(ulong);
        var chunks=raw.ReadChunky(header.BinaryDataPosition,chunkSize);
        
        var time = new ulong[header.NumberOfPoints];
        List<float[]> floats = [];
        for (var i = 0; i < numberOfFloats; i++)
            floats.Add(new float[header.NumberOfPoints]);
        var position = -1;
        foreach (var chunk in chunks)
        {
            position++;
            time[position]= BitConverter.ToUInt64(chunk);
            for (var i = 0; i < numberOfFloats; i++)
            {
                var startIndex=sizeof(long)+sizeof(float)*i;
                floats[i][position] = BitConverter.ToSingle(chunk,startIndex);
            }
        }

        return (time, floats);
    }
    
    #endregion
}
