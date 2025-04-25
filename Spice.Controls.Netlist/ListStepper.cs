namespace Spice.Controls.Netlist;

internal sealed record ListStepper(string Target, string[] Steps) : INetlistStepper
{
    public string ToNetlistLine() => string.Join(" ", [".steps", "param", Target, "list", ..Steps]);
}