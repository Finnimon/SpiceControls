namespace NetlistEditor;
public static class Measures
{
    private const string prefix = ".meas";
    public static void ClearAllMeasures(string path) => NetlistEditor.Core.ReplaceFileContents(path, File.ReadAllLines(path).Where(x => !x.StartsWith(prefix)));
}
