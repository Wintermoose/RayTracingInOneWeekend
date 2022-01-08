namespace RayTracingInOneWeekend;

using Ray = Mathematics.Ray;
using Vec3 = Mathematics.Vec3;
using Color = Mathematics.Vec3;

internal class Lambertian : IMaterial
{
    public Color Albedo { get; init; }

    public Lambertian( in Vec3 color )
    {
        Albedo = color;
    }

    public bool Scatter(Ray rIn, in HitRecord hit, out Vec3 attenuation, out Ray scattered)
    {
        var scatterDirection = hit.Normal + Vec3.RandomUnitVector();
        if ( scatterDirection.isNearZero() )
        {
            scatterDirection = hit.Normal;
        }

        scattered = new Ray(hit.P, scatterDirection);
        attenuation = Albedo;
        return true;
    }

}

