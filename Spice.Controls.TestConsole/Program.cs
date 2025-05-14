//using System.Globalization;
//using System.Text.Json;

var file = Console.ReadLine() ?? throw new PlatformNotSupportedException();
var raw = Spice.Controls.Raw.Reader.ReadSpiceRaw(file);

Console.WriteLine(raw.Header.OriginalHeader);

return 0;