using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetlistEditor;

public class SeparateFileConfig
{
    private List<(string param, double initial, double final, double step)> Steppers { get; set; }
    private List<(string param, object[] stepList)> ListSteppers { get; set; }
    private List<(string param, object val)> ParamList { get; set; }

    public SeparateFileConfig()
    {
        Steppers = [];
        ListSteppers = [];
        ParamList = [];
    }

    public void AddChangeParam(string param, object newVal) => ParamList.Add((param, newVal));
    public void AddStepper(string param, double initial, double final, double step) => Steppers.Add((param, initial, final, step));
    public void AddStepper(string param, object[] stepList) => ListSteppers.Add((param, stepList));

    public string[] RunAll(string netlist, string targetFolder)
    {
        var baseName = Path.GetFileName(netlist);
        baseName = Path.Combine(targetFolder, baseName);
        Core.AssertCopy(netlist, baseName);

        Stepper.ClearAllSteppers(baseName);
        Measures.ClearAllMeasures(baseName);
        foreach (var (param, newVal) in ParamList) Params.SetParam(baseName, param, newVal);
        
        List<string> rawFiles = [];
        var extension = Path.GetExtension(netlist);
        var extensionLess = baseName[..^extension.Length];
        foreach (var point in Steppers)
        {
            var (param, value, final, step) = point;
            while (value < final+0.01)
            {
                var name = $"{extensionLess}{param}is{value.ToString("F2", CultureInfo.CreateSpecificCulture("en-US"))}{extension}";
                Core.AssertCopy(baseName, name);
                Params.SetParam(name,param,value);
                rawFiles.Add(Runner.RunNetlist(name));
                value += step;
            }
        }
        foreach (var (param, stepList) in ListSteppers)
        {
            foreach (var value in stepList)
            {
                var name = $"{extensionLess}{param}is{value}{extension}";
                Core.AssertCopy(baseName, name);
                Params.SetParam(name, param, value);
                rawFiles.Add(Runner.RunNetlist(name));
            }
        }

        return rawFiles.ToArray();
    }
}