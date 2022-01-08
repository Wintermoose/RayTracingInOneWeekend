using RayTracingInOneWeekend;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Scenes;
internal class MetalGlassScene : IScene
{
    readonly double aspectRatio = 16.0 / 9.0;
    readonly Position _position;
    readonly GlassSphere _glassSphere;

    public enum Position
    {
        Front,
        Distant,
        DistantZoom,
        Blured
    }

    public enum GlassSphere
    {
        Solid,
        Thick,
        Thin
    }

    public MetalGlassScene( Position position = Position.Front, GlassSphere glassSphere = GlassSphere.Solid )
    {
        _position = position;
        _glassSphere = glassSphere;
    }

    public Camera GetCamera()
    {
        Point3 lookFrom = default( Point3 );
        double aperture = 0;
        double fov = 90;

        switch (_position)
        {
            case Position.Front:
                lookFrom = new(0, 0, 0);
                break;
            case Position.Distant:
                lookFrom = new(-2, 2, 1);
                break;
            case Position.DistantZoom:
                lookFrom = new(-2, 2, 1);
                fov = 20;
                break;
            case Position.Blured:
                lookFrom = new(3, 3, 2);
                fov = 20;
                aperture = 2.0;
                break;
        }
        Point3 lookAt = new(0, 0, -1);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = (lookFrom - lookAt).Length;
        return new(lookFrom, lookAt, vUp, fov, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 100, 50);
    }

    public IHittable GetWorld()
    {
        var world = new HittableList();
        var materialGround = new Lambertian(new Color(0.8, 0.8, 0.0));
        var materialCenter = new Lambertian(new Color(0.1, 0.2, 0.5));
        var materialLeft = new Dielectric(1.5);
        var materialRight = new Metal(new Color(0.8, 0.6, 0.2), 0.0);

        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100.0, materialGround));
        world.Add(new Sphere(new Point3(0.0, 0.0, -1.0), 0.5, materialCenter));
        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));
        switch (_glassSphere)
        {
            case GlassSphere.Solid:
                break;
            case GlassSphere.Thick:
                world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), -0.40, materialLeft));
                break;
            case GlassSphere.Thin:
                world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), -0.45, materialLeft));
                break;
        }
        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));
        return world;
    }
}
