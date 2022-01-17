using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Entities;

internal class XyRectangle : IHittable
{
    public XyRectangle( double x0, double x1, double y0, double y1, double z, IMaterial material )
    {
        _x0 = x0;
        _x1 = x1;
        _y0 = y0;
        _y1 = y1;
        _z = z;
        _material = material;
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        // we're infinitely small, but need to pad us a bit to return a real box
        return new Aabb(new Point3(_x0, _y0, _z - 0.0001), new Point3(_x1, _y1, _z + 0.0001));
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        if ( r.Direction.Z == 0 ) return null;
        var t = (_z - r.Origin.Z) / r.Direction.Z;
        if (t < tMin || t > tMax) return null;

        var x = r.Origin.X + t * r.Direction.X;
        var y = r.Origin.Y + t * r.Direction.Y;
        if (x < _x0 || x > _x1 || y < _y0 || y > _y1) return null;

        return new HitRecord(
            r.At(t),
            Vec3.UnitZ,
            t,
            _material,
            r,
            (x - _x0) / (_x1 - _x0),
            (y - _y0) / (_y1 - _y0)
            );
    }

    private IMaterial _material;
    private double _x0;
    private double _x1;
    private double _y0;
    private double _y1;
    private double _z;
}

