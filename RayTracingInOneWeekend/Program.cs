using RayTracingInOneWeekend;
using RayTracingInOneWeekend.Mathematics;
using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using RayTracingInOneWeekend.Entities;
using System.Diagnostics.CodeAnalysis;


void AssertIsNotNull([NotNull] object? nullableReference)
{
    if (nullableReference == null)
    {
        throw new ArgumentNullException();
    }
}


Color RayColor(Ray r, in Color background, IHittable world, int depth)
{
    if (depth <= 0) return Color.Zero;

    var hit = world.Hit(r, 0.001, double.MaxValue);
    if ( hit != null ) {
        Ray? scattered;
        Color attenuation;
        Color emitted = hit.Material.Emitted(hit.U, hit.V, hit.P);
        if (hit.Material.Scatter(r, hit, out attenuation, out scattered))
        {
            AssertIsNotNull(scattered);
            return emitted + attenuation * RayColor(scattered, background, world, depth - 1);
        }
        return emitted;
    } 
    else if ( background.X == -1 )
    {
        // Special value to mark the sky
        var unitDirection = r.Direction.UnitVector();
        var t = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - t) * new Color(1.0, 1.0, 1.0) + t * new Color(0.5, 0.7, 1.0);
    }
    else
    {
        return background;
    }
};


//var scene = new RayTracingInOneWeekend.Scenes.TwoLambertianSpheresScene();
//var scene = new RayTracingInOneWeekend.Scenes.RedBlueSphereScene();
//var scene = new RayTracingInOneWeekend.Scenes.GlossyMetalScene();
//var scene = new RayTracingInOneWeekend.Scenes.MetalGlassScene(RayTracingInOneWeekend.Scenes.MetalGlassScene.Position.DistantZoom, RayTracingInOneWeekend.Scenes.MetalGlassScene.GlassSphere.Thin);
//var scene = new RayTracingInOneWeekend.Scenes.Book1CoverScene();
//var scene = new RayTracingInOneWeekend.Scenes.Book1MovingScene(true);
//var scene = new RayTracingInOneWeekend.Scenes.TwoCheckeredSpheresScene();
//var scene = new RayTracingInOneWeekend.Scenes.TwoPerlinSphereScene(RayTracingInOneWeekend.Textures.NoiseTexture.Variant.Turbulence);
//var scene = new RayTracingInOneWeekend.Scenes.EarthScene();
//var scene = new RayTracingInOneWeekend.Scenes.SimpleLightScene(true);
//var scene = new RayTracingInOneWeekend.Scenes.CornellBoxScene(false);
//var scene = new RayTracingInOneWeekend.Scenes.CornellSmokeScene(false);
var scene = new RayTracingInOneWeekend.Scenes.Book2CoverScene();

var (aspectRatio, samplesPerPixel, maxDepth) = scene.GetPreferredParameters();

// Image
const int imageWidth = 1920;
//const int imageWidth = 400;
int imageHeight = (int)(imageWidth / aspectRatio);

// World
var worldSrc = scene.GetWorld();
var cam = scene.GetCamera();
var world = worldSrc.AsBvh(cam.Time0, cam.Time1);
var background = scene.GetBackground();

Console.WriteLine("P3");
Console.WriteLine($"{imageWidth} {imageHeight}");
Console.WriteLine("255");

var rnd = new Random();

var start = DateTime.Now;
for ( int j = imageHeight-1; j >= 0; j-- )
{
    Console.Error.Write($"\rScanlines remaining {j}   ");
    Console.Error.Flush();
    for (int i = 0; i < imageWidth; i++ )
    {
        var pixel = new Point3();
        for (int s = 0; s < samplesPerPixel; s++)
        {
            var u = (i + rnd.NextDouble()) / (imageWidth - 1);
            var v = (j + rnd.NextDouble()) / (imageHeight - 1);
            var r = cam.GetRay(u, v);
            pixel += RayColor(r, background, world, maxDepth);
        }
        Console.WriteLine(pixel.AsColorString( samplesPerPixel ));
    }
}
var end = DateTime.Now;
Console.Error.WriteLine($"\nRendering done in {(end - start).TotalSeconds} s");
