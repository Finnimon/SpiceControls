namespace NetlistEditor;
public static class Core
{
    private const string SpiceEnv = "SpicePath";

    public static string SpicePath()
        => Environment.GetEnvironmentVariable(SpiceEnv) ??
           throw new Exception($"Missing environment variable \"{SpiceEnv}\" leading to LTspice.exe");


    public static void ReplaceFileContents(string path, string newContent)
    {
        File.Delete(path);
        File.WriteAllText(path, newContent);
    }

    public static void ReplaceFileContents(string path, IEnumerable<string> newContent) => ReplaceFileContents(path, string.Join("\r\n", newContent));

    public static void AssertCopy(string from, string to)
    {
        if (File.Exists(to)) File.Delete(to);
        File.Copy(from, to);
    }
}
