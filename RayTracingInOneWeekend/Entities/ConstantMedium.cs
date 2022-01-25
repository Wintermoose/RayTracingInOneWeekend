using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Aabb = RayTracingInOneWeekend.Mathematics.Aabb;

using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Entities;
internal class ConstantMedium : IHittable
{
    public ConstantMedium(IHittable boundary, double density, in Color color )
        : this(boundary, density, new SolidColor(color))
    { }

    public ConstantMedium( IHittable boundary, double density, ITexture albedo)
    {
        _boundary = boundary;
        _negInverseDensity = (-1 / density);
        _phaseFunction = new Isotropic(albedo);
    }

    public Aabb? GetAabb(double time0, double time1) => _boundary.GetAabb(time0, time1);

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        // Print occasional samples when debugging. To enable, set enableDebug true.
        const bool enableDebug = false;
        bool debugging = enableDebug && Random.Shared.NextDouble() < 0.00001;

        var hit1 = _boundary.Hit(r, double.NegativeInfinity, double.PositiveInfinity);
        if (hit1 == null) return null;

        var hit2 = _boundary.Hit(r, hit1.T + 0.0001, double.PositiveInfinity);
        if (hit2 == null) return null;

        if (debugging) Console.Error.WriteLine( $"\nt_min={hit1.T} , t_max=${hit2.T}\n" );

        double t1 = Math.Max(hit1.T, Math.Max( 0, tMin ));
        double t2 = Math.Min(hit2.T, tMax);
        if (t1 >= t2) return null;

        var rayLength = r.Direction.Length;
        var distanceInsideBoundary = (t2 - t1) * rayLength;
        var hitDistance = _negInverseDensity * Math.Log(Random.Shared.NextDouble());

        if (hitDistance > distanceInsideBoundary) return null;

        var t = t1 + hitDistance / rayLength;
        var result = new HitRecord(
            r.At(t),
            Vec3.UnitX, // arbitrary
            t,
            _phaseFunction,
            r,
            0, 0   // arbitrary
            );

        if (debugging)
        {
            Console.Error.WriteLine($"hit_distance = ${hitDistance}\nt = ${result.T}\n P = ${result.P}\n");
        }

        return result;
    }

    private readonly IHittable _boundary;
    private readonly IMaterial _phaseFunction;
    private readonly double _negInverseDensity;
}
