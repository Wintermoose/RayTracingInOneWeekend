using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Textures;

internal interface ITexture
{
    Color Value(double u, double v, in Point3 point);
}
