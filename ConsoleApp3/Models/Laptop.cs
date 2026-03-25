namespace ConsoleApp3.Models;

public class Laptop : Equipment
{
    public int RamGb { get; set; }
    public string Processor { get; set; }

    public Laptop(string name, int ramGb, string processor) : base(name)
    {
        RamGb = ramGb;
        Processor = processor;
    }

    public override string GetDetails() =>
        $"Laptop: {Name} | RAM: {RamGb}GB | CPU: {Processor} | Status: {Status}";
}
