namespace NetlistEditor;
public static class Stepper
{
    public static void ClearAllSteppers(string path)
    {
        var lines = File.ReadAllLines(path).Where(x => !x.StartsWith(".step"));
        NetlistEditor.Core.ReplaceFileContents(path, lines);
    }


    public static void Step(string path, string paramName, object[] stepList)
    {
        var stepLine = $".step param {paramName} list {string.Join(" ", stepList.Select(x => x.ToString()))}";
        InternalStepperRoutine(path, paramName, stepLine);
    }

    public static void StepMultiple(string path, string[] paramNames, object[][] stepLists)
    {
        for (var i = 0; i < paramNames.Length; i++) Step(path, paramNames[i], stepLists[i]);
    }

    public static void Step(string path, string paramName, object initial, object final, object step)
    {
        var stepLine = $".step param {paramName} {initial} {final} {step}";
        InternalStepperRoutine(path, paramName, stepLine);
    }

    public static void StepMultiple(string path, string[] paramNames, object[] initial, object[] final, object[] step)
    {
        for (var i = 0; i < paramNames.Length; i++) Step(path, paramNames[i], initial[i], final[i], step[i]);
    }

    private static void InternalStepperRoutine(string path, string paramName, string stepLine)
    {
        var lines = File.ReadAllLines(path).ToList();
        AddStepLine(lines, paramName, stepLine);
        NetlistEditor.Core.ReplaceFileContents(path, lines);
    }
    private static void AddStepLine(List<string> lines, string paramName, string stepLine)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (!line.StartsWith(".step")) continue;
            if (!line.Contains(paramName)) continue;
            lines[i] = line;
            return;
        }

        lines.Insert(lines.Count - 2, stepLine);
    }

}
