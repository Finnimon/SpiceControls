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
   
    private static SpiceRawHeader ReadHeader(FileStream file)
    {
        file.Seek(0, SeekOrigin.Begin);
        var firstIsZero = file.ReadByte() == 0;
        var secondIsZero = file.ReadByte() == 0;
        var isUtf16 = firstIsZero || secondIsZero;
        file.Seek(0, SeekOrigin.Begin);
        var headerStr= file.ReadUntilFound("Binary:\n",isUtf16,secondIsZero);
        var binaryDataOffset = headerStr.Length;
        if (isUtf16) binaryDataOffset *= 2;
        var title=new StringReader(headerStr).ReadLine()?["Title: * ".Length..]??"Unknown";
        return new SpiceRawHeader(title,ExtractMeasurements(headerStr,out var numberOfPoints),numberOfPoints,binaryDataOffset,headerStr);
    }

    private static SpiceMeasurement[] ExtractMeasurements(string entireHeader, out int numberOfPoints)
    {
        entireHeader = entireHeader.Replace('\t', ' ');
        var lines=entireHeader.Split('\n',StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
        numberOfPoints=int.Parse(lines.First(x => x.StartsWith("No. Points:"))["No. Points:".Length..].Trim());
        var noVars=int.Parse(lines.First(x=>x.StartsWith("No. Variables:"))["No. Variables:".Length..].Trim());
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
            var name=relevant.Substring(2,relevant.Length-2);
            measurements.Add(new SpiceMeasurement(name,unit));
        }
        return measurements.ToArray();
    }

    #endregion
    #region Read Body

    private static (ulong[] time, Dictionary<SpiceMeasurement, float[]> measurements) ReadBody(FileStream raw, SpiceRawHeader header)
    {
        var (time, measurementValues) = ReadBinary(raw, header);
        Dictionary<SpiceMeasurement, float[]> measurements = [];
        for (var i = 0; i < header.Measurements.Length-1; i++)
        {
            var meas = header.Measurements[i+1];
            measurements[meas] = measurementValues[i];
        }
        return (time, measurements);
    }
    
    private static (ulong[] time, List<float[]> measurements) ReadBinary(FileStream raw, SpiceRawHeader header)
    {
        raw.Seek(header.BinaryDataOffset, SeekOrigin.Begin);
        var numberOfFloats = header.Measurements.Length - 1;
        var chunkSize = sizeof(float) * numberOfFloats + sizeof(ulong);
        var chunks=raw.ReadChunky(chunkSize, header.NumberOfPoints);
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
