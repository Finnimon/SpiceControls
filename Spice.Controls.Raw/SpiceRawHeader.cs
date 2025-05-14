using Spice.Controls.Core;

namespace Spice.Controls.Raw;

public sealed record SpiceRawHeader(
    string Title,
    DateTime Date,
    string PlotName,
    string[] Flags,
    int NumberOfVariables,
    int NumberOfPoints,
    double Offset,
    string Command,
    SpiceMeasurement[] Variables,
    FilePosition BinaryDataPosition,
    string OriginalHeader
);