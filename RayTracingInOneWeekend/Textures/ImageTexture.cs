using Color = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracingInOneWeekend.Textures;

internal class ImageTexture : ITexture, IDisposable
{
    public ImageTexture( string filename ) {
        _image = Image.Load<RgbaVector>(filename);
    }

    public Color Value(double u, double v, in Color point)
    {
        u = Math.Clamp(u, 0, 1);
        v = 1 - Math.Clamp(v, 0, 1);

        // Note: cannot multiply * (Width - 1), because we wouldn't display the whole image this way
        var i = (int)(u * _image.Width);
        var j = (int)(v * _image.Height);

        // Clamp integer mapping, since actual coordinates should be less than 1.0
        if (i >= _image.Width) i = _image.Width - 1;
        if (j >= _image.Height) j = _image.Height - 1;

        var pixel = _image[i, j];
        return new Color(pixel.R, pixel.G, pixel.B);
    }

    public void Dispose() => _image.Dispose();

    private Image<RgbaVector> _image;
}
