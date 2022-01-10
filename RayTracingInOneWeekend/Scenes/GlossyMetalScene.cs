using RayTracingInOneWeekend;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;

namespace RayTracingInOneWeekend.Scenes;
internal class GlossyMetalScene : IScene
{
    readonly double aspectRatio = 16.0 / 9.0;

    public Camera GetCamera()
    {
        Point3 lookFrom = lookFrom = new(0, 0, 0);
        double aperture = 0;
        double fov = 90;
        Point3 lookAt = new(0, 0, -1);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = (lookFrom - lookAt).Length;
        return new(lookFrom, lookAt, vUp, fov, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 100, 50);
    }

    public HittableList GetWorld()
    {
        var world = new HittableList();
        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.7, 0.3, 0.3));
        var materialLeft = new Metal(new Color(0.8, 0.8, 0.8), 0.3);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 1.0);

        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100.0, materialGround));
        world.Add(new Sphere(new Point3(0.0, 0.0, -1.0), 0.5, materialCenter));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));
        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));
        return world;
    }
}
