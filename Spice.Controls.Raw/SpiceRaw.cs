namespace Spice.Controls.Raw;

public sealed record SpiceRaw(SpiceRun[] Steps);

public sealed record SpiceRun(
    SpiceParam[] Params, 
    long[] Time, 
    IReadOnlyDictionary<string,double> Data);
public sealed record SpiceParam(string Id, string Value);