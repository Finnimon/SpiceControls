namespace NetlistEditor;

public class Config
{
    private List<(string param, object initial, object final, object step)> Steppers { get; set; }
    private List<(string param, object[] stepList)> ListSteppers { get; set; }
    private List<(string param, object val)> ParamList { get; set; }

    public Config()
    {
        Steppers = [];
        ListSteppers = [];
        ParamList = [];
    }

    public void AddChangeParam(string param, object newVal) => ParamList.Add((param, newVal));
    public void AddStepper(string param, object initial, object final, object step) => Steppers.Add((param, initial, final, step));
    public void AddStepper(string param, object[] stepList) => ListSteppers.Add((param, stepList));

    public void Apply(string path)
    {
        Stepper.ClearAllSteppers(path);
        Measures.ClearAllMeasures(path);
        foreach (var (param, initial, final, step) in Steppers) Stepper.Step(path, param, initial, final, step);
        foreach (var (param, stepList) in ListSteppers) Stepper.Step(path, param, stepList);
        foreach (var (param, newVal) in ParamList) Params.SetParam(path, param, newVal);
    }


    public void ApplyOnCopy(string sourcePath, string targetPath)
    {
        if (File.Exists(targetPath)) File.Delete(targetPath);
        File.Copy(sourcePath, targetPath);
        Apply(targetPath);
    }
}
