//var test=File.ReadAllText(
//    "C:\\Users\\flindig\\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\\Dokumente\\HSB\\WiSe24-25\\SEELEK\\lab\\B6 HSS\\Sim\\matlab\\TEMP\\testii3.cir");
//Console.WriteLine(test);
var netlist =
    @"C:\Users\flindig\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\Dokumente\HSB\WiSe24-25\SEELEK\lab\B6 EWR\MatLab\B6 EWR.net";
var targetFolder =
    @"C:\Users\flindig\OneDrive - SWMS Systemtechnik Ingenieurges. mbH\Dokumente\HSB\WiSe24-25\SEELEK\lab\B6 HSS\Sim\matlab\TEMP";
var targetName = "testii";

NetlistEditor.Runner.RunNetlist(netlist);
