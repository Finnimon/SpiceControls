using Spice.Controls.Netlist;

namespace Spice.Controls.Raw;

public sealed record SpiceRaw(
    SpiceRawHeader Header, 
    ulong[] Time, 
    Dictionary<SpiceMeasurement, float[]> Measurements);


