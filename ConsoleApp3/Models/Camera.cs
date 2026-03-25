namespace ConsoleApp3.Models;

public class Camera : Equipment
{
    public double MegaPixels { get; set; }
    public string LensType { get; set; }

    public Camera(string name, double megaPixels, string lensType) : base(name)
    {
        MegaPixels = megaPixels;
        LensType = lensType;
    }

    public override string GetDetails() =>
        $"Camera: {Name} | MP: {MegaPixels} | Lens: {LensType} | Status: {Status}";
}
