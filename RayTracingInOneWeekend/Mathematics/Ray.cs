namespace RayTracingInOneWeekend.Mathematics;

using System.Runtime.CompilerServices;
using Point3 = Vec3;
internal class Ray
{
    public Ray(Ray other)
    {
        Origin = other.Origin;
        Direction = other.Direction;
    }

    public Ray(Point3 origin, Vec3 direction, double time = 0.0 )
    {
        Origin = origin;
        Direction = direction;
        Time = time;
    }

    public readonly Point3 Origin;

    public readonly Vec3 Direction;

    public readonly double Time;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point3 At(double t)
    {
        return Origin + t * Direction;
    }

}
