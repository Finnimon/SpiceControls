using System.Text.Json;

var file = Console.ReadLine() ?? throw new PlatformNotSupportedException();

var ser = new JsonSerializerOptions { WriteIndented = true };
var raw = Spice.Controls.Raw.Reader.ReadSpiceRaw(file);
Console.WriteLine(JsonSerializer.Serialize(raw.Header));