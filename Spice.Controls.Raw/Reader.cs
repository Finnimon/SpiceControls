using System.Text;
using Microsoft.VisualBasic.FileIO;
using Spice.Controls.Core;

namespace Spice.Controls.Raw;

public static class Reader
{
    private static SpiceRawHeader ReadHeader(FileStream file)
    {
        file.Seek(0, SeekOrigin.Begin);
        var first=file.ReadByte();
        var isUtf16 = first == 0;
        file.Seek(0, SeekOrigin.Begin);
        var headerStr= file.ReadUntilFound("Binary:\n",isUtf16);
        var binaryDataOffset = headerStr.Length;
        if (isUtf16) binaryDataOffset *= 2;
        var title=new StringReader(headerStr).ReadLine()?.Split(" ").Last()??"Unknown";
        return new SpiceRawHeader(title,ExtractMeasurements(headerStr),binaryDataOffset,headerStr);
    }

    private static SpiceMeasurement[] ExtractMeasurements(string entireHeader)
        => throw new NotImplementedException();
}

public sealed record SpiceMeasurement(string Id, ElectricUnit Unit);

public enum ElectricUnit
{
    Voltage,
    Ampere
}

public sealed record SpiceRawHeader(
    string Title,
    SpiceMeasurement[] Measurements,
    long BinaryDataOffset,
    string OriginalHeader
);
