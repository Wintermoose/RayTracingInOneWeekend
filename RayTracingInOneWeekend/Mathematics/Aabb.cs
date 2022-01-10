using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Mathematics;
internal class Aabb
{
    public Point3 Min { get; init; }
    public Point3 Max { get; init; }

    public Aabb( in Point3 min, in Point3 max )
    {
        Min = min;
        Max = max;
    }

    public bool Hit(Ray r, double tMin, double tMax) {
        for (int a = 0; a< 3; a++) {
            var invD = 1.0 / r.Direction[a];
            var t0 = (Min[a] - r.Origin[a]) * invD;
            var t1 = (Max[a] - r.Origin[a]) * invD;

            if (invD < 0.0f) {
                var temp = t0;
                t0 = t1;
                t1 = temp;
            }

            tMin = t0 > tMin? t0 : tMin;
            tMax = t1 < tMax? t1 : tMax;
            if (tMax <= tMin) return false;
        }
        return true;
    }

    public static Aabb SurroundingBox(in Aabb box0, in Aabb box1)
    {
        Point3 small = Point3.Min(box0.Min, box1.Min);
        Point3 big = Point3.Max(box0.Max, box1.Max);
        return new Aabb(small, big);
    }
}
