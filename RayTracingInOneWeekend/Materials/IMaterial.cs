using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;

namespace RayTracingInOneWeekend.Materials;

internal interface IMaterial
{ 
    bool Scatter(Ray rIn, in HitRecord hit, out Color attenuation, out Ray? scattered);

    Color Emitted(double u, double v, in Point3 p) {
        return Color.Zero;
    }
}
