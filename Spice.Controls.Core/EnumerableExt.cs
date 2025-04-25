namespace Spice.Controls.Core;

public static class EnumerableExt
{
    public static IEnumerable<T> Range<T>(this IEnumerable<T> me, int startIncl, int endExcl)
    => me.Take(new Range(startIncl, endExcl));
    
}