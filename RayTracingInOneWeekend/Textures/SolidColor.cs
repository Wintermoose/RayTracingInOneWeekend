using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Textures;

internal class SolidColor : ITexture
{
    public SolidColor( in Color color )
    {
        _color = color;
    }

    public SolidColor( double red, double green, double blue )
        : this( new Color(red, green, blue) )
    { }

    public Color Value(double u, double v, in Point3 point)
    {
        return _color;
    }

    private Color _color;
}
