using Spice.Controls.Core;

namespace Spice.Controls.Raw;

public static class Reader
{
    public static SpiceRawHeader ReadHeader(string file)
    {
        using var reader=File.OpenRead(file);
        return ReadHeader(reader);
    }
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
}