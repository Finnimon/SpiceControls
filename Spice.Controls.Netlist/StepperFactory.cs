using Spice.Controls.Core;

namespace Spice.Controls.Netlist;

public static class StepperFactory
{
    public static IEnumerable<INetlistStepper> GetSteppers(string stepLine)
    {
        if (!stepLine.StartsWith(".step")) yield break;
        var split = stepLine.Split(' ');
        List<int> paramPos = [];
        for (var i = 1; i < split.Length; i++)
            if (split[i].Equals("param"))
                paramPos.Add(i);
        if (paramPos.Count == 0) yield break;
        paramPos.Add(split.Length);
        for (var i = 0; i < paramPos.Count - 1; i++)
        {
            var start = paramPos[i];
            var end = paramPos[i + 1];
            var stepper = split.Range(start, end).ToList();
            yield return ExtractStepper(stepper);
        }
    }

    private static INetlistStepper ExtractStepper(List<string> stepper)
    {
        var mode =Function.Try(GetMode,stepper);
        return mode switch
        {
            StepperMode.Unknown => throw new NotSupportedException(
                $"Stepper mode {mode} is not supported.\n{string.Join(" ", stepper)}"),
            StepperMode.List=>new ListStepper(stepper[1], stepper.Skip(3).ToArray()),
            StepperMode.Range=>new RangeStepper(stepper[1], stepper[2], stepper[3], stepper[4]),
            _=>throw new NotSupportedException(
                $"Stepper mode {mode} is not supported.\n{string.Join(" ", stepper)}")
        };
    }

    private static StepperMode GetMode(List<string> stepper)
    {
        if(stepper.Count == 0) return StepperMode.Unknown;
        if(stepper[2].Equals("list")) return StepperMode.List;
        var dVal=double.TryParse(stepper[2],out _);
        if( dVal) return StepperMode.Range;
        return StepperMode.Unknown;
    }

    private enum StepperMode
    {
        Unknown,
        List,
        Range
    }
}