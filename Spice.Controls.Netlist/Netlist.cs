namespace Spice.Controls.Netlist;

public sealed class Netlist 
{
    private FileInfo File { get; }
    private List<string> Net { get; set; }

    private Dictionary<string, NetlistParameter> Params { get; }
    private Dictionary<string, INetlistStepper> Steppers { get; }

    public Netlist(FileInfo file)
    {
        File = file;
        Net = System.IO.File.ReadAllLines(file.FullName).ToList();
        Params = GetParams();
        Steppers = GetSteppers();
    }

    private Dictionary<string, INetlistStepper> GetSteppers()
        => Net.Where(x=>x.StartsWith(".step"))
            .SelectMany(StepperFactory.GetSteppers)
            .ToDictionary(x=>x.Target);

    private Dictionary<string, NetlistParameter> GetParams()
        => Net.Where(x => x.StartsWith(".param"))
            .SelectMany(x => x.Split(' ',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries))
            .Where(x => x.Contains('='))
            .Select(x => x.Split('=',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries))
            .Where(x => x.Length == 2)
            .Select(x => new NetlistParameter(x[0], x[1]))
            .ToLookup(x => x.Id)
            .ToDictionary(x => x.Key, x => x.Single());

    public void SaveChanges()
    {
        UpdateNetlist();
        File.Delete();
        using var writer = new StreamWriter(File.FullName);
        foreach (var line in Net) writer.WriteLine(line);
    }

    private void UpdateNetlist()
    {
        Net=Net.Where(x => !(x.StartsWith(".param") || x.StartsWith(".step"))).ToList();
        List<INetlistLine>insert=[..Params.Values,..Steppers.Values];
        Net.InsertRange(Net.Count-2, insert.Select(x=>x.ToNetlistLine()));
    }

}