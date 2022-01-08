namespace RayTracingInOneWeekend;
using Ray = Mathematics.Ray;
using Color = Mathematics.Vec3;

internal interface IMaterial
{ 
    bool Scatter(Ray rIn, in HitRecord hit, out Color attenuation, out Ray scattered);
}
