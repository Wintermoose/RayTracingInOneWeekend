using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Entities;

internal class YzRectangle : IHittable
{
    public YzRectangle( double y0, double y1, double z0, double z1, double x, IMaterial material )
    {
        _y0 = y0;
        _y1 = y1;
        _z0 = z0;
        _z1 = z1;
        _x = x;
        _material = material;
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        // we're infinitely small, but need to pad us a bit to return a real box
        return new Aabb(new Point3(_x - 0.0001, _y0, _z0 ), new Point3(_x + 0.0001, _y1, _z1 ));
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        if ( r.Direction.X == 0 ) return null;
        var t = (_x - r.Origin.X) / r.Direction.X;
        if (t < tMin || t > tMax) return null;

        var y = r.Origin.Y + t * r.Direction.Y;
        var z = r.Origin.Z + t * r.Direction.Z;
        if (y < _y0 || y > _y1 || z < _z0 || z > _z1) return null;

        return new HitRecord(
            r.At(t),
            Vec3.UnitX,
            t,
            _material,
            r,
            (y - _y0) / (_y1 - _y0),
            (z - _z0) / (_z1 - _z0)
            );
    }

    private IMaterial _material;
    private double _y0;
    private double _y1;
    private double _z0;
    private double _z1;
    private double _x;
}

