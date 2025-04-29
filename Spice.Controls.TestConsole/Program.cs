using System.Text.Json;

// var raw = Console.ReadLine()??throw new NullReferenceException();
var file = "/home/finnimon/Downloads/B6_HSS.raw";


var raw= Spice.Controls.Raw.Reader.ReadSpiceRaw(file);
