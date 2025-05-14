using System.Runtime.CompilerServices;

namespace Spice.Controls.Core;

public static class ListExt
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool EndsWith<T>(this IList<T> list, IList<T> other)
    {
        if (other.Count > list.Count) return false;
        var otherI = other.Count;
        var until = list.Count - other.Count;
        for (var i = list.Count - 1; i >= until; i--)
        {
            otherI--;
            var item = list[i];
            var otherItem = other[otherI];
            var equals = item?.Equals(otherItem) ?? item is null && otherItem is null;
            if (!equals) return false;
        }

        return true;
    }


}