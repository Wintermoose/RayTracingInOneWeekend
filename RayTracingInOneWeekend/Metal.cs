namespace RayTracingInOneWeekend;

using Ray = Mathematics.Ray;
using Vec3 = Mathematics.Vec3;
using Color = Mathematics.Vec3;

internal class Metal : IMaterial
{
    public Color Albedo { get; init; }
    public double Fuzz { get; init; }

    public Metal(in Vec3 color, double fuzz)
    {
        Albedo = color;
        Fuzz = Math.Min(fuzz, 1);
    }
    public bool Scatter(Ray rIn, in HitRecord hit, out Vec3 attenuation, out Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(rIn.Direction.UnitVector(), hit.Normal);
        scattered = new Ray(hit.P, reflected + Fuzz * Vec3.RandomInUnitSphere() );
        attenuation = Albedo;
        return Vec3.Dot(scattered.Direction, hit.Normal) > 0;
    }

}
