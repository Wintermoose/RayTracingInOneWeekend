using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using RayTracingInOneWeekend.Entities;

namespace RayTracingInOneWeekend.Scenes;

internal interface IScene
{
    HittableList GetWorld();

    Camera GetCamera();

    (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters();

    Color GetBackground();
}

