using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Materials;
internal class DiffuseLight : IMaterial
{
    public DiffuseLight(ITexture texture)
    {
        _emit = texture;
    }

    public DiffuseLight(Color color)
    {
        _emit = new SolidColor(color);
    }

    public bool Scatter(Ray rIn, in HitRecord hit, out Color attenuation, out Ray? scattered)
    {
        attenuation = default;
        scattered = null;
        return false;
    }

    public Color Emitted(double u, double v, in Point3 p)
    {
        return _emit.Value(u, v, p);
    }

    private readonly ITexture _emit;
}
