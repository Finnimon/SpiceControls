namespace Spice.Controls.Netlist;

internal sealed record RangeStepper(string Target, string Initial, string Final, string Step) : INetlistStepper
{
    public string ToNetlistLine() => string.Join(" ", ".step", "param", Target, Initial, Final, Step);
}