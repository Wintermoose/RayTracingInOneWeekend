using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Scenes;
internal class SimpleLightScene: IScene
{
    readonly double aspectRatio = 4.0 / 3;

    public SimpleLightScene( bool includeBallLight )
    {
        _includeBallLight = includeBallLight;
    }

    public Camera GetCamera()
    {
        Point3 lookFrom = new(26, 3, 6);
        Point3 lookAt = new(0, 2, 0);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = 10;
        double aperture = 0;
        return new(lookFrom, lookAt, vUp, 20.0, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 400, 50);
    }

    public Color GetBackground()
    {
        return new Color(0.0, 0.0, 0);
    }

    public HittableList GetWorld()
    {
        var world = new HittableList();

        var texture = new NoiseTexture(NoiseTexture.Variant.Marble, 4);
        IMaterial lambertian = new Lambertian(texture);
        world.Add(new Sphere(new Point3(0, -1000, 0), 1000, lambertian));
        world.Add(new Sphere(new Point3(0, 2, 0), 2, lambertian));

        var diffuseLight = new DiffuseLight(new Color(4, 4, 4));
        world.Add(new XyRectangle(3, 5, 1, 3, -2, diffuseLight));

        if (_includeBallLight )
        {
            var diffuseLight2 = new DiffuseLight(new Color(2, 2, 4));
            world.Add(new Sphere(new Point3(0, 7, 0), 2, diffuseLight2));
        }
        return world;
    }

    private readonly bool _includeBallLight;
}

