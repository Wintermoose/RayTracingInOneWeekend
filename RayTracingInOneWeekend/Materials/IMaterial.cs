using Ray = RayTracingInOneWeekend.Mathematics.Ray;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;

namespace RayTracingInOneWeekend.Materials;

internal interface IMaterial
{ 
    bool Scatter(Ray rIn, in HitRecord hit, out Color attenuation, out Ray scattered);
}
