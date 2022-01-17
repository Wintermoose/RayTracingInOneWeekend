using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

namespace RayTracingInOneWeekend.Entities;
internal class Translate: IHittable
{
    public Translate( IHittable entity, in Vec3 offset )
    {
        _entity = entity;
        _offset = offset;
    }


    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        var movedRay = new Ray(r.Origin - _offset, r.Direction, r.Time);
        var hit = _entity.Hit(movedRay, tMin, tMax);
        if (hit == null) return null;

        return new HitRecord(hit.P + _offset, hit.Normal, hit.T, hit.Material, movedRay, hit.U, hit.V);
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        var inner = _entity.GetAabb(time0, time1);
        if (inner == null) return null;

        return new Aabb(inner.Min + _offset, inner.Max + _offset);
    }

    private readonly IHittable _entity;
    private readonly Vec3 _offset;
}

