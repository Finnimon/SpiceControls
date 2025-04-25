namespace Spice.Controls.Core;

public static class ListExt
{
    public static int? FindIndex<T, TList>(this TList list, T find)
    where TList : IList<T>
    {
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if(item is null&&find is null) return i;
            if(item is null) continue;
            if (item.Equals(find)) return i;
        }
        return null;
    }
}