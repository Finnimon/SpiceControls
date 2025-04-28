namespace Spice.Controls.Core;

public static class Function
{
    public static ILogger? Logger { get; set; } = null;
    public static T? Try<T>(Func<T> obj, T? defaultTo = default)
    {
        try
        {
            return obj();
        }
        catch (Exception ex)
        {
            Logger?.Log(ex);
            return defaultTo;
        }
    }

    public static T? Try<TArg,T>(Func<TArg,T> obj, TArg arg, T? defaultTo = default)
    {
        try
        {
            return obj(arg);
        }
        catch (Exception ex) 
        {
            Logger?.Log(ex);
            return defaultTo;
        }
    }

    public static bool Try<T>(Func<T> obj, out T? result, T? defaultTo = default)
    {
        try
        {
            result = obj();
            return true;
        }
        catch (Exception e)
        {
            Logger?.Log(e);
            result = defaultTo;
            return false;
        }
    }
    
    public static T? TryWithFallback<T>(Func<T> obj,T? defaultTo = default, params Func<T>[] fallbacks) 
    => Try(obj, out var result) || fallbacks.Any(fallback => !Try(fallback, out result)) ? result : defaultTo;
}
