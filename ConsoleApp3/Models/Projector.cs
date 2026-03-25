namespace ConsoleApp3.Models;

public class Projector : Equipment
{
    public int Lumens { get; set; }
    public string Resolution { get; set; }

    public Projector(string name, int lumens, string resolution) : base(name)
    {
        Lumens = lumens;
        Resolution = resolution;
    }

    public override string GetDetails() =>
        $"Projector: {Name} | Lumens: {Lumens} | Resolution: {Resolution} | Status: {Status}";
}
