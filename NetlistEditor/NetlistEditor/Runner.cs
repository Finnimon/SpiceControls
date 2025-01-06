using System.Diagnostics;

namespace NetlistEditor;
public static class Runner
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="netlistPath"></param>
    /// <returns>The Location of the RAW File</returns>
    public static string RunNetlist(string netlistPath)
    {
        var process= Process.Start(NetlistEditor.Core.SpicePath(), ["-b", netlistPath]);
        while (!process.HasExited)
        {
            Thread.Sleep(100);
        }
        return Path.ChangeExtension(netlistPath, "raw");
    }


    public static string RunNetlist(string netlistPath, string paramName, object newVal) => RunNetlist(netlistPath, [paramName], [newVal]);


    public static string RunNetlist(string netlistPath, string[] paramNames, object[] newVals, int tempNameAppend = 1)
    {
        var temp = Environment.GetEnvironmentVariable("TEMP") ?? throw new Exception("TEMP EnvVar missing");
        var tempDir = new DirectoryInfo(Path.Combine(temp, "SpiceMultiParametric", $"nr{tempNameAppend}"));
        if (!tempDir.Exists) tempDir.Create();
        var fileName = Path.Combine(tempDir.FullName, $"{Path.GetFileNameWithoutExtension(netlistPath)}{tempNameAppend}{Path.GetExtension(netlistPath)}");
        if (File.Exists(fileName)) File.Delete(fileName);
        File.Copy(netlistPath, fileName);
        fileName.SetParams(paramNames, newVals);
        return RunNetlist(fileName);
    }


    public static string[] RunNetlist(string netListPath, string[][] paramNames, object[][] newVals)
    {
        var outFileNames = new string[paramNames.Length];
        for (var i = 0; i < paramNames.Length; i++) outFileNames[i] = RunNetlist(netListPath, paramNames[i], newVals[i], i);
        return outFileNames;
    }

    public static string[] RunNetlist(string netlistPath, string targetFolder, string targetName, string param, object[] newVals)
    {
        var rawPaths = new string[newVals.Length];
        if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
        for (var i = 0; i < newVals.Length; i++)
        {
            var path = Path.Combine(targetFolder, $"{targetName}{i}.cir");
            
            Core.AssertCopy(netlistPath, path);
            path.SetParam(param, newVals[i]);
            rawPaths[i] = RunNetlist(path);
        }

        return rawPaths;
    }
}
