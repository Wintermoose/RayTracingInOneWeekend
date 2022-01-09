namespace RayTracingInOneWeekend;

using Ray = Mathematics.Ray;
using Vec3 = Mathematics.Vec3;
using Color = Mathematics.Vec3;

internal class Dielectric : IMaterial
{
    public double IndexOfRefraction { get; init; }

    public Dielectric(double indexOfRefraction)
    {
        IndexOfRefraction = indexOfRefraction;
    }

    public bool Scatter(Ray rIn, in HitRecord hit, out Vec3 attenuation, out Ray scattered)
    {
        attenuation = new Color(1, 1, 1);
        double refractionRatio = hit.FrontFace ? (1.0 / IndexOfRefraction) : IndexOfRefraction;
        Vec3 unitDirection = rIn.Direction.UnitVector();

        double cosTheta = Math.Min(-Vec3.Dot(unitDirection, hit.Normal), 1.0);
        double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

        bool cannotRefract = refractionRatio * sinTheta > 1.0;
        Vec3 direction;

        if (cannotRefract || Reflectance(cosTheta, refractionRatio) > System.Random.Shared.NextDouble())
            direction = Vec3.Reflect(unitDirection, hit.Normal);
        else
        {
            direction = Vec3.Refract(unitDirection, hit.Normal, refractionRatio);
            //if ( !hit.FrontFace )
            //{
            //    double att = (hit.P - rIn.Origin).Length;
            //    att = Math.Max(0, 1 - 0.8 * Math.Pow(att, 2));
            //    attenuation = new Color(0.8 * att, att, 0.8 * att);
            //}
        }

        scattered = new Ray(hit.P, direction, rIn.Time);
        return true;
    }

    static double Reflectance(double cosine, double refractionIndex)
    {
        // Use Schlick's approximation for reflectance.
        var r0 = (1 - refractionIndex) / (1 + refractionIndex);
        r0 = r0 * r0;
        return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
    }

}
