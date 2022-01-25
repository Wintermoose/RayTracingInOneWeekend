using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Scenes;

internal class Book2CoverScene : IScene
{
    readonly double aspectRatio = 1.0;
    public Camera GetCamera()
    {
        Point3 lookFrom = new(478, 278, -600);
        Point3 lookAt = new(278, 278, 0);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = 10;
        double aperture = 0;
        return new(lookFrom, lookAt, vUp, 40.0, aspectRatio, aperture, distToFocus, 0, 1);
    }

    public (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters()
    {
        return (aspectRatio, 10000, 50);
     //   return (aspectRatio, 100, 50);
    }

    public Color GetBackground()
    {
        return new Color(0, 0, 0);
    }

    public HittableList GetWorld()
    {
        var world = new HittableList();

        var boxes1 = new HittableList();
        var ground = new Lambertian(new Color(0.48, 0.83, 0.53));

        const int boxes_per_side = 20;
        for (int i = 0; i < boxes_per_side; i++)
        {
            for (int j = 0; j < boxes_per_side; j++)
            {
                var w = 100.0;
                var x0 = -1000.0 + i * w;
                var z0 = -1000.0 + j * w;
                var y0 = 0.0;
                var x1 = x0 + w;
                var y1 = 1 + 100 * Random.Shared.NextDouble();
                var z1 = z0 + w;

                boxes1.Add(new Box(new Point3(x0, y0, z0), new Point3(x1, y1, z1), ground));
            }
        }

        world.Add(boxes1.AsBvh(0, 1));

        var light = new DiffuseLight(new Color(7, 7, 7));
        world.Add(new XzRectangle(123, 423, 147, 412, 554, light));

        var center1 = new Point3(400, 400, 200);
        var center2 = center1 + new Vec3(30, 0, 0);
        var moving_sphere_material = new Lambertian(new Color(0.7, 0.3, 0.1));
        world.Add(new MovingSphere(center1, center2, 0, 1, 50, moving_sphere_material));

        world.Add(new Sphere(new Point3(260, 150, 45), 50, new Dielectric(1.5)));
        world.Add(new Sphere(
            new Point3(0, 150, 145), 50, new Metal(new Color(0.8, 0.8, 0.9), 1.0)
        ));

        var boundary = new Sphere(new Point3(360, 150, 145), 70, new Dielectric(1.5));
        world.Add(boundary);
        world.Add(new ConstantMedium(boundary, 0.2, new Color(0.2, 0.4, 0.9)));
        boundary = new Sphere(new Point3(0, 0, 0), 5000, new Dielectric(1.5));
        world.Add(new ConstantMedium(boundary, .0001, new Color(1, 1, 1)));

        var emat = new Lambertian(new ImageTexture("Assets/earthmap.jpg"));
        world.Add(new Sphere(new Point3(400, 200, 400), 100, emat));
        var pertext = new NoiseTexture(NoiseTexture.Variant.Marble, 0.1);
        world.Add(new Sphere(new Point3(220, 280, 300), 80, new Lambertian(pertext)));

        var boxes2 = new HittableList();
        var white = new Lambertian(new Color(.73, .73, .73));
        int ns = 1000;
        for (int j = 0; j < ns; j++)
        {
            boxes2.Add(new Sphere(Point3.Random(0, 165), 10, white));
        }

        world.Add(new Translate(
            new RotateY(boxes2.AsBvh( 0.0, 1.0), 15),
            new Vec3(-100, 270, 395)
        ));

        return world;
    }

}

