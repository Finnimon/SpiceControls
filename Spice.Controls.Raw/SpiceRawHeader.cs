namespace Spice.Controls.Raw;

public sealed record SpiceRawHeader(
    string Title,
    SpiceMeasurement[] Measurements,
    int NumberOfPoints,
    long BinaryDataOffset,
    string OriginalHeader
);