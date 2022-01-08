using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;

namespace RayTracingInOneWeekend;

record class HitRecord
{
    public Point3 P { get; }
    public Vec3 Normal { get; }
    public double T { get; }
    public IMaterial Material { get; }

    public bool FrontFace { get; }

    public HitRecord( Point3 p, Vec3 outwardNormal, double t, IMaterial material, Ray r)
    {
        P = p;
        T = t;
        Material = material;
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}


interface IHittable
{
    HitRecord? Hit(in Ray r, double tMin, double tMax);
}