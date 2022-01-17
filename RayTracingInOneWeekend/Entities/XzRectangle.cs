using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Entities;

internal class XzRectangle : IHittable
{
    public XzRectangle( double x0, double x1, double z0, double z1, double y, IMaterial material )
    {
        _x0 = x0;
        _x1 = x1;
        _z0 = z0;
        _z1 = z1;
        _y = y;
        _material = material;
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        // we're infinitely small, but need to pad us a bit to return a real box
        return new Aabb(new Point3(_x0, _y - 0.0001, _z0 ), new Point3(_x1, _y + 0.0001, _z1 ));
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        if ( r.Direction.Y == 0 ) return null;
        var t = (_y - r.Origin.Y) / r.Direction.Y;
        if (t < tMin || t > tMax) return null;

        var x = r.Origin.X + t * r.Direction.X;
        var z = r.Origin.Z + t * r.Direction.Z;
        if (x < _x0 || x > _x1 || z < _z0 || z > _z1) return null;

        return new HitRecord(
            r.At(t),
            Vec3.UnitY,
            t,
            _material,
            r,
            (x - _x0) / (_x1 - _x0),
            (z - _z0) / (_z1 - _z0)
            );
    }

    private IMaterial _material;
    private double _x0;
    private double _x1;
    private double _z0;
    private double _z1;
    private double _y;
}

