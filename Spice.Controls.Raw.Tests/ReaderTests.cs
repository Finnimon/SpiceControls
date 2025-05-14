using System.Text.Json;
using NUnit.Framework;

namespace Spice.Controls.Raw.Tests;

public class ReaderTests
{
    [TestCase(RawFile0, HeaderJson0)]
    [TestOf(typeof(Reader))]
    public void TestReadHeader(string rawFile, string expectedHeaderJson)
    {
        using var reader = File.OpenRead(rawFile);
        var header = Reader.ReadHeader(reader);
        var expectedHeader = JsonSerializer.Deserialize<SpiceRawHeader>(expectedHeaderJson)??throw new ArgumentNullException(nameof(expectedHeaderJson));
        Assert.That(expectedHeader.Title, Is.EqualTo(header.Title));
        Assert.That(expectedHeader.Date, Is.EqualTo(header.Date));
        Assert.That(expectedHeader.PlotName, Is.EqualTo(header.PlotName));
        Assert.That(expectedHeader.NumberOfVariables, Is.EqualTo(header.NumberOfVariables));
        Assert.That(expectedHeader.NumberOfPoints, Is.EqualTo(header.NumberOfPoints));
        Assert.That(expectedHeader.Offset, Is.EqualTo(header.Offset));
        Assert.That(expectedHeader.Command, Is.EqualTo(header.Command));
        Assert.That(expectedHeader.PlotName, Is.EqualTo(header.PlotName));
        Assert.That(expectedHeader.BinaryDataPosition,Is.EqualTo(header.BinaryDataPosition));
        Assert.That(expectedHeader.OriginalHeader, Is.EqualTo(header.OriginalHeader));
        Assert.IsTrue(expectedHeader.Variables.SequenceEqual(header.Variables));
        Assert.IsTrue(expectedHeader.Flags.SequenceEqual(header.Flags));
    }

    private const string RawFile0 = "Files/B6_HSS-UTF16LE.raw";

    private const string HeaderJson0 =
        "{\"Title\":\"C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 HSS\\\\Sim\\\\ltspice\\\\B6 HSS.asc\",\"Date\":\"2025-01-13T03:36:09\",\"PlotName\":\"Transient Analysis\",\"Flags\":[\"real\",\"forward\"],\"NumberOfVariables\":14,\"NumberOfPoints\":8544,\"Offset\":0,\"Command\":\"Linear Technology Corporation LTspice\",\"Variables\":[{\"Id\":\"time\",\"Unit\":0},{\"Id\":\"ue\",\"Unit\":1},{\"Id\":\"n001\",\"Unit\":1},{\"Id\":\"n002\",\"Unit\":1},{\"Id\":\"n005\",\"Unit\":1},{\"Id\":\"n004\",\"Unit\":1},{\"Id\":\"n003\",\"Unit\":1},{\"Id\":\"C\",\"Unit\":2},{\"Id\":\"D1\",\"Unit\":2},{\"Id\":\"L\",\"Unit\":2},{\"Id\":\"Rv\",\"Unit\":2},{\"Id\":\"S\",\"Unit\":2},{\"Id\":\"Ue_src\",\"Unit\":2},{\"Id\":\"Pwm\",\"Unit\":2}],\"BinaryDataPosition\":{\"Offset\":1322,\"Length\":512640,\"Origin\":0},\"OriginalHeader\":\"Title: * C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 HSS\\\\Sim\\\\ltspice\\\\B6 HSS.asc\\nDate: Mon Jan 13 03:36:09 2025\\nPlotname: Transient Analysis\\nFlags: real forward\\nNo. Variables: 14\\nNo. Points:         8544\\nOffset:    0.0000000000000000e\\u002B00\\nCommand: Linear Technology Corporation LTspice\\nVariables:\\n 0 time time\\n 1 V(ue) voltage\\n 2 V(n001) voltage\\n 3 V(n002) voltage\\n 4 V(n005) voltage\\n 5 V(n004) voltage\\n 6 V(n003) voltage\\n 7 I(C) device_current\\n 8 I(D1) device_current\\n 9 I(L) device_current\\n 10 I(Rv) device_current\\n 11 I(S) device_current\\n 12 I(Ue_src) device_current\\n 13 I(Pwm) device_current\\nBinary:\\n\"}";
}