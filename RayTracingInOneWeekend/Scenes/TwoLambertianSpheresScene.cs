using RayTracingInOneWeekend;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Scenes;
internal class TwoLambertianSpheresScene: IScene
{
    readonly double aspectRatio = 16.0 / 9.0;
    public Camera GetCamera()
    {
        Point3 lookFrom = new(0, 0, 0);
        Point3 lookAt = new(0, 0, -1);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = (lookFrom - lookAt).Length;
        double aperture = 0;
        return new(lookFrom, lookAt, vUp, 90.0, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 100, 50);
    }

    public IHittable GetWorld()
    {
        var world = new HittableList();
        IMaterial lambertian = new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5, lambertian));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100, lambertian));
        return world;
    }
}

