using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;

namespace RayTracingInOneWeekend;

internal class Sphere : IHittable { 
    public Point3 Center { get; init; }
    public double Radius { get; init; }
    public IMaterial Material { get; init; }

    public Sphere(in Point3 center, double radius, IMaterial material)
    {
        Center = center;
        Radius = radius;
        Material = material;
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        var oc = r.Origin - Center;

        var a = r.Direction.LengthSquared;
        var halfB = Vec3.Dot(oc, r.Direction);
        var c = oc.LengthSquared - Radius * Radius;
        var discriminant = halfB * halfB - a * c;

        if (discriminant < 0)
        {
            return null;
        }

        var sqrtD = Math.Sqrt(discriminant);
        // find nearest root in the acceptable range
        var root = (-halfB - sqrtD) / a;
        if ( root < tMin || root > tMax )
        {
            root = (-halfB + sqrtD) / a;
            if (root < tMin || root > tMax)
            {
                return null;
            }
        }

        Vec3 p = r.At(root);
        Vec3 outwardNormal = (p - Center) / Radius;
        return new HitRecord(p, outwardNormal, root, Material, r);
    }
}