using System.Globalization;

namespace NetlistEditor;
public static class Params
{
    public static void SetParam(this string path, string param, object newVal) => SetParams(path, [param], [newVal]);

    public static void SetParams(this string path, string[] paramNames, object[] newVals)
    {
        var lines = File.ReadAllLines(path);
        for (var index = 0; index < lines.Length; index++)
        {
            lines[index] = TryEditLine(lines[index], paramNames, newVals);
        }

        NetlistEditor.Core.ReplaceFileContents(path, lines);
    }

    private static string TryEditLine(this string line, string[] paramNames, object[] newVals)
    {
        if (!line.StartsWith(".param")) return line;
        var split = line.Split(" ");

        for (var i = 0; i < paramNames.Length; i++) line = TryChangeParam(line, split, paramNames[i], newVals[i]);

        return line;
    }

    private static string TryChangeParam(string line, string[] splitLine, string paramName, object newVal)
    {
        var changed = false;
        for (var i = 0; i < splitLine.Length; i++)
        {
            var current = splitLine[i];
            if (!current.StartsWith($"{paramName}=")) continue;
            changed = true;
            var newValString = newVal is not (double or float) 
                ? newVal.ToString() 
                : ((double)newVal).ToString("F2", CultureInfo.CreateSpecificCulture("en-US"));
            splitLine[i] = $"{paramName}={newValString}";
        }

        return !changed ? line : string.Join(" ", splitLine);
    }
}
