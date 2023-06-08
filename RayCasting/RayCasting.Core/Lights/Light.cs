using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using System.Drawing;

namespace RayCasting.Core.Lights;

public abstract class Light
{
    protected readonly Color Color;
    protected readonly float Intensity;

    protected Light(Color color, float intensity)
    {
        Color = color;
        if (intensity is < 0 or > 1)
            throw new ArgumentOutOfRangeException("Intensity value is out of range");
        Intensity = intensity;
    }

    protected Light(float intensity) : this(Color.White, intensity)
    {
    }

    public virtual Color GetPixel(Point3 point, IObject figure, List<IObject> objects)
    {
        var coeff = 1.0f;
        coeff *= Intensity;
        return Color.FromArgb(
            Color.A,
            (byte)Math.Round(Color.R * coeff, MidpointRounding.AwayFromZero),
            (byte)Math.Round(Color.G * coeff, MidpointRounding.AwayFromZero),
            (byte)Math.Round(Color.B * coeff, MidpointRounding.AwayFromZero));
    }
}