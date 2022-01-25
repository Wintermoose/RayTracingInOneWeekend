using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Scenes;
internal class CornellSmokeScene: IScene
{
    readonly double aspectRatio = 1;

    public CornellSmokeScene(bool aligned)
    {
        _aligned = aligned;
    }

    public Camera GetCamera()
    {
        Point3 lookFrom = new(278, 278, -800);
        Point3 lookAt = new(278, 278, 0);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = 10;
        double aperture = 0;
        return new(lookFrom, lookAt, vUp, 40.0, aspectRatio, aperture, distToFocus);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 4000, 50);
//        return (aspectRatio, 200, 50);
    }

    public Color GetBackground()
    {
        return new Color(0.0, 0.0, 0);
    }

    public HittableList GetWorld()
    {
        var world = new HittableList();

        var red = new Lambertian(new Color(.65, .05, .05));
        var white = new Lambertian(new Color(.73, .73, .73));
        var green = new Lambertian(new Color(.12, .45, .15));
        var light = new DiffuseLight(new Color(7, 7, 7));

        world.Add(new YzRectangle(0, 555, 0, 555, 555, green));
        world.Add(new YzRectangle(0, 555, 0, 555, 0, red));
        world.Add(new XzRectangle(113, 443, 127, 432, 554, light));
        world.Add(new XzRectangle(0, 555, 0, 555, 0, white));
        world.Add(new XzRectangle(0, 555, 0, 555, 555, white));
        world.Add(new XyRectangle(0, 555, 0, 555, 555, white));

        IHittable box1 = new Box(new Point3(0, 0, 0), new Point3(165, 330, 165), white);
        if ( !_aligned )
        {
            box1 = new RotateY(box1, 15);
        }
        box1 = new Translate(box1, new Vec3(265, 0, 295));
        world.Add(new ConstantMedium(box1, 0.01, new Color(0,0,0) ) );

        IHittable box2 = new Box(new Point3(0, 0, 0), new Point3(165, 165, 165), white);
        if ( !_aligned)
        {
            box2 = new RotateY(box2, -18);
        }
        box2 = new Translate(box2, new Vec3(130, 0, 65));
        world.Add(new ConstantMedium(box2, 0.01, new Color(1,1,1) ) );
        return world;
    }

    private readonly bool _aligned;
}

