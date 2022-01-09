using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;

namespace RayTracingInOneWeekend;

internal class MovingSphere : Sphere
{
    public MovingSphere(
        in Point3 center0, 
        in Point3 center1,
        double time0,
        double time1,
        double radius, IMaterial material): base(center0, radius, material)
    {
        _center1 = center1;
        _time0 = time0;
        _time1 = time1;
    }

    public override Aabb? GetAabb(double time0, double time1)
    {
        var box0 = new Aabb(
            GetCenter(time0) - new Vec3(Radius),
            GetCenter(time0) + new Vec3(Radius)
        );
        var box1 = new Aabb(
            GetCenter(time1) - new Vec3(Radius),
            GetCenter(time1) + new Vec3(Radius)
        );
        return Aabb.SurroundingBox(box0, box1);
    }

    protected override Point3 GetCenter(double time) => Center + ((time - _time0) / (_time1 - _time0)) * (_center1 - Center);

    private Point3 _center1;
    private double _time0;
    private double _time1;
}
