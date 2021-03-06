using RayTracingInOneWeekend;
using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using RayTracingInOneWeekend.Materials;
using RayTracingInOneWeekend.Textures;

namespace RayTracingInOneWeekend.Scenes;

internal class Book1MovingScene : IScene
{
    readonly double aspectRatio = 16.0 / 9.0;
    
    public Book1MovingScene( bool checkered )
    {
        _checkered = checkered;
    }

    public Camera GetCamera()
    {
        Point3 lookFrom = new(13, 2, 3);
        Point3 lookAt = new(0, 0, 0);
        Vec3 vUp = new(0, 1, 0);
        double distToFocus = 10;
        double aperture = 0.1;
        return new(lookFrom, lookAt, vUp, 20.0, aspectRatio, aperture, distToFocus, 0.0, 1.0);
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

        var groundMaterial = _checkered ? 
            new Lambertian( new CheckerTexture(new Color(0.2, 0.3, 0.1), new Color(0.9, 0.9, 0.9)) ) : 
            new Lambertian(new Color(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Point3(0, -1000, 0), 1000, groundMaterial));
        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                var choose_mat = Random.Shared.NextDouble();
                Point3 center = new(a + 0.9 * Random.Shared.NextDouble(), 0.2, b + 0.9 * Random.Shared.NextDouble());

                if ((center - new Point3(4, 0.2, 0)).Length > 0.9)
                {
                    IMaterial sphere_material;

                    if (choose_mat < 0.8)
                    {
                        // diffuse
                        var albedo = Color.Random() * Color.Random();
                        sphere_material = new Lambertian(albedo);
                        var center2 = center + new Vec3(0, 0.5 * Random.Shared.NextDouble(), 0);
                        world.Add(new MovingSphere(center, center2, 0.0, 1.0, 0.2, sphere_material));
                    }
                    else if (choose_mat < 0.95)
                    {
                        // metal
                        var albedo = Color.Random(0.5, 1);
                        var fuzz = Random.Shared.NextDouble() * 0.5;
                        sphere_material = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphere_material));
                    }
                    else
                    {
                        // glass
                        sphere_material = new Dielectric(1.5);
                        world.Add(new Sphere(center, 0.2, sphere_material));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5);
        world.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

        var material2 = new Lambertian(new Color(0.4, 0.2, 0.1));
        world.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

        var material3 = new Metal(new Color(0.7, 0.6, 0.5), 0.0);
        world.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));
        return world;
    }

    private readonly bool _checkered;

}

