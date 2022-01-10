using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Entities;

record class HitRecord
{
    public Point3 P { get; }
    public Vec3 Normal { get; }
    public double T { get; }
    public IMaterial Material { get; }

    public double U { get; }
    public double V { get; }

    public bool FrontFace { get; }

    public HitRecord( Point3 p, Vec3 outwardNormal, double t, IMaterial material, Ray r, double u, double v)
    {
        P = p;
        T = t;
        Material = material;
        FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
        U = u;
        V = v;
    }
}


interface IHittable
{
    HitRecord? Hit(in Ray r, double tMin, double tMax);
    Aabb? GetAabb(double time0, double time1);
}