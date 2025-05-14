using Spice.Controls.Netlist;

namespace Spice.Controls.Raw;

public sealed record SpiceRaw(
    SpiceRawHeader Header, 
    double[] Time, 
    Dictionary<string, float[]> Measurements);



