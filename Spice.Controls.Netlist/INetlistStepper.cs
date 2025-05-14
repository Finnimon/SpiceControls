namespace Spice.Controls.Netlist;

public interface INetlistStepper : INetlistLine
{
    public string Target { get; }
}