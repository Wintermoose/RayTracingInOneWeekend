using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Materials;

internal class Lambertian : IMaterial
{
    public ITexture Albedo { get; init; }

    public Lambertian( in Vec3 color )
    {
        Albedo = new SolidColor( color );
    }

    public Lambertian(ITexture texture)
    {
        Albedo = texture;
    }

    public bool Scatter(Ray rIn, in HitRecord hit, out Vec3 attenuation, out Ray scattered)
    {
        var scatterDirection = hit.Normal + Vec3.RandomUnitVector();
        if ( scatterDirection.isNearZero() )
        {
            scatterDirection = hit.Normal;
        }

        scattered = new Ray(hit.P, scatterDirection, rIn.Time);
        attenuation = Albedo.Value(hit.U, hit.V, hit.P);
        return true;
    }

}

