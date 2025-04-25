namespace Spice.Controls.Core;

public readonly record struct FilePosition(long Offset, long Length, SeekOrigin Origin=SeekOrigin.Begin);