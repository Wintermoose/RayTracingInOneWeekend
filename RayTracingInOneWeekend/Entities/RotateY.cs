using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

namespace RayTracingInOneWeekend.Entities;
internal class RotateY: IHittable
{
    public RotateY( IHittable entity, double angle )
    {
        _entity = entity;
        double radians = Math.PI * angle / 180.0;
        _sinTheta = Math.Sin( radians );
        _cosTheta = Math.Cos( radians );       
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        var origin = r.Origin;
        var rotatedOrigin = new Point3(_cosTheta * origin.X - _sinTheta * origin.Z, origin.Y, _sinTheta * origin.X + _cosTheta * origin.Z);
        var direction = r.Direction;
        var rotatedDirection = new Vec3(_cosTheta * direction.X - _sinTheta * direction.Z, direction.Y, _sinTheta * direction.X + _cosTheta * direction.Z);
        var rotatedR = new Ray(rotatedOrigin, rotatedDirection, r.Time);

        var hit = _entity.Hit(rotatedR, tMin, tMax);
        if (hit == null) return null;

        var p = hit.P;
        var rotatedP = new Point3(_cosTheta * p.X + _sinTheta * p.Z, p.Y, -_sinTheta * p.X + _cosTheta * p.Z);
        var normal = hit.Normal;
        var rotatedNormal = new Vec3(_cosTheta * normal.X + _sinTheta * normal.Z, normal.Y, -_sinTheta * normal.X + _cosTheta * normal.Z);

        return new HitRecord(rotatedP, rotatedNormal, hit.T, hit.Material, r, hit.U, hit.V);
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        var inner = _entity.GetAabb(time0, time1);
        if (inner == null) return null;

        Vec3 min = new Vec3(double.PositiveInfinity);
        Vec3 max = new Vec3(double.NegativeInfinity);
        for  ( int i=0; i < 2; ++i )
        {
            double x = i == 0 ? inner.Min.X : inner.Max.X;
            for ( int j=0; j < 2; ++j )
            {
                double y = j == 0 ? inner.Min.Y : inner.Max.Y;
                for ( int k = 0; k < 2; ++k ){
                    double z = k == 0 ? inner.Min.Z : inner.Max.Z;
                    var newx = _cosTheta * x + _sinTheta * z;
                    var newz = -_sinTheta * x + _cosTheta * z;
                    var rotated = new Vec3(newx, y, newz);
                    min = Vec3.Min(min, rotated);
                    max = Vec3.Max(max, rotated);
                }
            }
        }
        return new Aabb(min, max);
    }

    private readonly IHittable _entity;
    private readonly double _sinTheta;
    private readonly double _cosTheta;
}
