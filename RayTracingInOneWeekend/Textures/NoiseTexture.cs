using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Textures;

internal class NoiseTexture : ITexture
{
    public enum Variant {
        Perlin,
        Turbulence,
        Marble
    }

    public NoiseTexture( Variant variant, double scale = 1)
    {
        _variant = variant;
        _scale = scale;
    }

    public Color Value(double u, double v, in Point3 point)
    {
        switch ( _variant )
        {
            default:
            case Variant.Perlin:
                return new Color(1) * 0.5 * (1.0 + _perlin.Noise(_scale * point));
            case Variant.Turbulence:
                return new Color(1) * _perlin.Turbulence(_scale * point);
            case Variant.Marble:
                return new Color(1) * 0.5 * (1 + Math.Sin(_scale * point.Z + 10 * _perlin.Turbulence(point)));
        }
    }

    private readonly Mathematics.Perlin3d _perlin = new();
    private readonly Variant _variant;
    private readonly double _scale;
}
