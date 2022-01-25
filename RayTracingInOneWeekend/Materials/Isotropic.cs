using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Materials;

internal class Isotropic : IMaterial
{
    public Isotropic( in Color color )
    {
        _albedo = new SolidColor(color);
    }

    public Isotropic( ITexture texture )
    {
        _albedo = texture;
    }

    public bool Scatter(Ray rIn, in HitRecord hit, out Color attenuation, out Ray? scattered)
    {
        scattered = new Ray(hit.P, Vec3.RandomInUnitSphere(), rIn.Time);
        attenuation = _albedo.Value(hit.U, hit.V, hit.P);
        return true;
    }

    private readonly ITexture _albedo;
}
