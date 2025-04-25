namespace Spice.Controls.Netlist;

public sealed record NetlistParameter(string Id, string Value):INetlistLine
{
    public string ToNetlistLine()=>$".param {Id}={Value}";
}