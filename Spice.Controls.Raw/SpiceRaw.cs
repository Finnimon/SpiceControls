using Spice.Controls.Netlist;

namespace Spice.Controls.Raw;

public sealed record SpiceRaw(SpiceRun[] Runs);

public sealed record SpiceRun(
    NetlistParameter[] Params, 
    long[] Time, 
    IReadOnlyDictionary<string,double> Data
);