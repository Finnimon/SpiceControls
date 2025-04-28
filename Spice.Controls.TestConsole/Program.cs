using System.Text.Json;

var header=Spice.Controls.Raw.Reader.ReadHeader(Console.ReadLine()??throw new NullReferenceException());
#pragma warning disable CA1869
var jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
#pragma warning restore CA1869
var ser=JsonSerializer.Serialize(header,jsonSerializerOptions);
Console.WriteLine(ser);