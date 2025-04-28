using System.Runtime.CompilerServices;

namespace Spice.Controls.Core;

public static class ListExt
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool EndsWith<T>(this IList<T> list,IList<T> other)
    {
        if(other.Count>list.Count) return false;
        var otherI = other.Count;
        var until=list.Count-other.Count;
        for (var i = list.Count - 1; i >= until; i--)
        {
            otherI--;
            var item=list[i];
            var otherItem = other[otherI];
            if(item is null && otherItem is null) continue;
            if(item is null) return false;
            if(!item.Equals(otherItem)) return false;
        }
        return true;
    }
}