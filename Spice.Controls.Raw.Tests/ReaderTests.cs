using System.Text.Json;
using NUnit.Framework;

namespace Spice.Controls.Raw.Tests;

public class ReaderTests
{
    [TestCase(RawFile2,HeaderJson2)]
    [TestCase(RawFile1,HeaderJson1)]
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

    private const string RawFile2 = "Files/B6_TSS_Spannungsregler.raw";

    private const string HeaderJson2 =
        "{\"Title\":\"C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 TSS\\\\Sim\\\\B6 TSS Spannungsregler.asc\",\"Date\":\"2024-12-02T05:08:27\",\"PlotName\":\"Transient Analysis\",\"Flags\":[\"real\",\"forward\",\"stepped\"],\"NumberOfVariables\":20,\"NumberOfPoints\":40010,\"Offset\":0.01,\"Command\":\"Linear Technology Corporation LTspice\",\"Variables\":[{\"Id\":\"time\",\"Unit\":0},{\"Id\":\"n002\",\"Unit\":1},{\"Id\":\"ua\",\"Unit\":1},{\"Id\":\"ue\",\"Unit\":1},{\"Id\":\"sw_on\",\"Unit\":1},{\"Id\":\"n001\",\"Unit\":1},{\"Id\":\"n004\",\"Unit\":1},{\"Id\":\"n003\",\"Unit\":1},{\"Id\":\"p001\",\"Unit\":1},{\"Id\":\"C\",\"Unit\":2},{\"Id\":\"D1\",\"Unit\":2},{\"Id\":\"Vreg\",\"Unit\":2},{\"Id\":\"L\",\"Unit\":2},{\"Id\":\"Vreg_pre\",\"Unit\":2},{\"Id\":\"R1\",\"Unit\":2},{\"Id\":\"S1\",\"Unit\":2},{\"Id\":\"Lastswitch\",\"Unit\":2},{\"Id\":\"U\",\"Unit\":2},{\"Id\":\"Switchcontroller\",\"Unit\":2},{\"Id\":\"Lastswitchcontroller\",\"Unit\":2}],\"BinaryDataPosition\":{\"Offset\":1718,\"Length\":3360840,\"Origin\":0},\"OriginalHeader\":\"Title: * C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 TSS\\\\Sim\\\\B6 TSS Spannungsregler.asc\\nDate: Mon Dec  2 05:08:27 2024\\nPlotname: Transient Analysis\\nFlags: real forward stepped\\nNo. Variables: 20\\nNo. Points:        40010\\nOffset:    1.0000000000000000e-02\\nCommand: Linear Technology Corporation LTspice\\nVariables:\\n 0 time time\\n 1 V(n002) voltage\\n 2 V(ua) voltage\\n 3 V(ue) voltage\\n 4 V(sw_on) voltage\\n 5 V(n001) voltage\\n 6 V(n004) voltage\\n 7 V(n003) voltage\\n 8 V(p001) voltage\\n 9 I(C) device_current\\n 10 I(D1) device_current\\n 11 I(Vreg) device_current\\n 12 I(L) device_current\\n 13 I(Vreg_pre) device_current\\n 14 I(R1) device_current\\n 15 I(S1) device_current\\n 16 I(Lastswitch) device_current\\n 17 I(U) device_current\\n 18 I(Switchcontroller) device_current\\n 19 I(Lastswitchcontroller) device_current\\nBinary:\\n\"}";
    private const string RawFile1 = "Files/B6_TSS.raw";
    private const string HeaderJson1 = "{\"Title\":\"C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 TSS\\\\Sim\\\\B6 TSS.asc\",\"Date\":\"2025-01-12T22:42:07\",\"PlotName\":\"Transient Analysis\",\"Flags\":[\"real\",\"forward\"],\"NumberOfVariables\":15,\"NumberOfPoints\":3493,\"Offset\":0.08,\"Command\":\"Linear Technology Corporation LTspice\",\"Variables\":[{\"Id\":\"time\",\"Unit\":0},{\"Id\":\"n002\",\"Unit\":1},{\"Id\":\"ua\",\"Unit\":1},{\"Id\":\"ue\",\"Unit\":1},{\"Id\":\"sw_on\",\"Unit\":1},{\"Id\":\"n001\",\"Unit\":1},{\"Id\":\"C\",\"Unit\":2},{\"Id\":\"C1\",\"Unit\":2},{\"Id\":\"D1\",\"Unit\":2},{\"Id\":\"L\",\"Unit\":2},{\"Id\":\"R1\",\"Unit\":2},{\"Id\":\"R_last\",\"Unit\":2},{\"Id\":\"S1\",\"Unit\":2},{\"Id\":\"U\",\"Unit\":2},{\"Id\":\"Switchcontroller\",\"Unit\":2}],\"BinaryDataPosition\":{\"Offset\":1390,\"Length\":223552,\"Origin\":0},\"OriginalHeader\":\"Title: * C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 TSS\\\\Sim\\\\B6 TSS.asc\\nDate: Sun Jan 12 22:42:07 2025\\nPlotname: Transient Analysis\\nFlags: real forward\\nNo. Variables: 15\\nNo. Points:         3493\\nOffset:    8.0000000000000002e-02\\nCommand: Linear Technology Corporation LTspice\\nVariables:\\n 0 time time\\n 1 V(n002) voltage\\n 2 V(ua) voltage\\n 3 V(ue) voltage\\n 4 V(sw_on) voltage\\n 5 V(n001) voltage\\n 6 I(C) device_current\\n 7 I(C1) device_current\\n 8 I(D1) device_current\\n 9 I(L) device_current\\n 10 I(R1) device_current\\n 11 I(R_last) device_current\\n 12 I(S1) device_current\\n 13 I(U) device_current\\n 14 I(Switchcontroller) device_current\\nBinary:\\n\"}";
    private const string RawFile0 = "Files/B6_HSS-UTF16LE.raw";
    private const string HeaderJson0 = "{\"Title\":\"C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 HSS\\\\Sim\\\\ltspice\\\\B6 HSS.asc\",\"Date\":\"2025-01-13T03:36:09\",\"PlotName\":\"Transient Analysis\",\"Flags\":[\"real\",\"forward\"],\"NumberOfVariables\":14,\"NumberOfPoints\":8544,\"Offset\":0,\"Command\":\"Linear Technology Corporation LTspice\",\"Variables\":[{\"Id\":\"time\",\"Unit\":0},{\"Id\":\"ue\",\"Unit\":1},{\"Id\":\"n001\",\"Unit\":1},{\"Id\":\"n002\",\"Unit\":1},{\"Id\":\"n005\",\"Unit\":1},{\"Id\":\"n004\",\"Unit\":1},{\"Id\":\"n003\",\"Unit\":1},{\"Id\":\"C\",\"Unit\":2},{\"Id\":\"D1\",\"Unit\":2},{\"Id\":\"L\",\"Unit\":2},{\"Id\":\"Rv\",\"Unit\":2},{\"Id\":\"S\",\"Unit\":2},{\"Id\":\"Ue_src\",\"Unit\":2},{\"Id\":\"Pwm\",\"Unit\":2}],\"BinaryDataPosition\":{\"Offset\":1322,\"Length\":512640,\"Origin\":0},\"OriginalHeader\":\"Title: * C:\\\\Users\\\\flindig\\\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\\\Dokumente\\\\HSB\\\\WiSe24-25\\\\SEELEK\\\\lab\\\\B6 HSS\\\\Sim\\\\ltspice\\\\B6 HSS.asc\\nDate: Mon Jan 13 03:36:09 2025\\nPlotname: Transient Analysis\\nFlags: real forward\\nNo. Variables: 14\\nNo. Points:         8544\\nOffset:    0.0000000000000000e\\u002B00\\nCommand: Linear Technology Corporation LTspice\\nVariables:\\n 0 time time\\n 1 V(ue) voltage\\n 2 V(n001) voltage\\n 3 V(n002) voltage\\n 4 V(n005) voltage\\n 5 V(n004) voltage\\n 6 V(n003) voltage\\n 7 I(C) device_current\\n 8 I(D1) device_current\\n 9 I(L) device_current\\n 10 I(Rv) device_current\\n 11 I(S) device_current\\n 12 I(Ue_src) device_current\\n 13 I(Pwm) device_current\\nBinary:\\n\"}";
}