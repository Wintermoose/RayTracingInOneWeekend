using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Scenes;
internal class EarthScene: IScene
{
    readonly double aspectRatio = 16.0 / 9.0;
    public Camera GetCamera()
    {
        Point3 lookFrom = new(13, 2, 3);
        Point3 lookAt = new(0, 0, 0);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = (lookFrom - lookAt).Length;
        double aperture = 0;
        return new(lookFrom, lookAt, vUp, 20.0, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 100, 50);
    }

    public Color GetBackground()
    {
        // use sky
        return new Color(-1, 0, 0);
    }

    public HittableList GetWorld()
    {
        var world = new HittableList();
        var earthTexture = new ImageTexture("Assets/earthmap.jpg");
        IMaterial lambertian = new Lambertian(earthTexture);
        world.Add(new Sphere(new Point3(0, 0, 0), 2, lambertian));
        return world;
    }
}

