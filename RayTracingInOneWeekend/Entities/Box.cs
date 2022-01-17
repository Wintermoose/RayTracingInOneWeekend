using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Entities;

internal class Box : IHittable
{
    public Box( in Point3 p0, in Point3 p1, IMaterial material )
    {
        _boxMin = p0;
        _boxMax = p1;
        _sides = new();
        _sides.Add(new XyRectangle(p0.X, p1.X, p0.Y, p1.Y, p1.Z, material));
        _sides.Add(new XyRectangle(p0.X, p1.X, p0.Y, p1.Y, p0.Z, material));
        _sides.Add(new XzRectangle(p0.X, p1.X, p0.Z, p1.Z, p1.Y, material));
        _sides.Add(new XzRectangle(p0.X, p1.X, p0.Z, p1.Z, p0.Y, material));
        _sides.Add(new YzRectangle(p0.Y, p1.Y, p0.Z, p1.Z, p1.X, material));
        _sides.Add(new YzRectangle(p0.Y, p1.Y, p0.Z, p1.Z, p0.X, material));
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        return new Aabb(_boxMin, _boxMax);
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax) => _sides.Hit(r, tMin, tMax);

    private readonly Point3 _boxMin;
    private readonly Point3 _boxMax;
    private readonly HittableList _sides;
}

