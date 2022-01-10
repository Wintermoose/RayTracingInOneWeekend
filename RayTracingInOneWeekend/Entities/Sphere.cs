using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;
namespace RayTracingInOneWeekend.Entities;

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
        var center = GetCenter(r.Time);
        var oc = r.Origin - center;

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
        Vec3 outwardNormal = (p - center) / Radius;
        var (u, v) = GetSphereUv(outwardNormal);
        return new HitRecord(p, outwardNormal, root, Material, r, u, v);
    }

    public virtual Aabb? GetAabb(double time0, double time1) => new Aabb(
        Center - new Vec3(Radius),
        Center + new Vec3(Radius)
    );

    protected virtual Point3 GetCenter(double time)
    {
        return Center;
    }

    static (double u, double v) GetSphereUv(in Point3 p)
    {
        // p: a given point on the sphere of radius one, centered at the origin.
        // u: returned value [0,1] of angle around the Y axis from X=-1.
        // v: returned value [0,1] of angle from Y=-1 to Y=+1.
        //     <1 0 0> yields <0.50 0.50>       <-1  0  0> yields <0.00 0.50>
        //     <0 1 0> yields <0.50 1.00>       < 0 -1  0> yields <0.50 0.00>
        //     <0 0 1> yields <0.25 0.50>       < 0  0 -1> yields <0.75 0.50>

        double theta = Math.Acos(-p.Y);
        double phi = Math.Atan2(-p.Z, p.X) + Math.PI;

        return (u: phi / (2 * Math.PI), v: theta / Math.PI);
    }
            
}