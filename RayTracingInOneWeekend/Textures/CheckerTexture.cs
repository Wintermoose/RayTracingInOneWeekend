using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Textures;

internal class CheckerTexture : ITexture
{
    public CheckerTexture( ITexture odd, ITexture even )
    {
        _odd = odd;
        _even = even;
    }

    public CheckerTexture( in Color odd, in Color even )
        : this( new SolidColor(odd), new SolidColor(even) )
    { }

    public Color Value(double u, double v, in Point3 point)
    {
        double sines = Math.Sin(10 * point.X) * Math.Sin(10 * point.Y) * Math.Sin(10 * point.Z);
        if (sines < 0)
        {
            return _odd.Value(u, v, point);
        }
        else
        {
            return _even.Value(u, v, point);
        }
    }

    private ITexture _odd;
    private ITexture _even;

}
