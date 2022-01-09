using Vec3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;

namespace RayTracingInOneWeekend;
internal class Camera
{
    public Camera(
        in Point3 lookFrom,
        in Point3 lookAt,
        in Vec3 up,
        double vFov, 
        double aspectRatio,
        double aperture,
        double focusDist,
        double time0 = 0,
        double time1 = 0
    )
    {
        var theta = Math.PI * vFov / 180;
        var h = Math.Tan(theta / 2);
        var viewportHeight = 2.0 * h;
        var viewportWidth = aspectRatio * viewportHeight;

        _w = (lookFrom - lookAt).UnitVector();
        _u = Vec3.Cross(up, _w).UnitVector();
        _v = Vec3.Cross(_w, _u);

        _origin = lookFrom;
        _horizontal = focusDist * viewportWidth * _u;
        _vertical = focusDist * viewportHeight * _v;
        _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focusDist * _w;

        _lensRadius = aperture / 2;
        _time0 = time0;
        _time1 = time1;
    }

    public Ray GetRay(double s, double t)
    {
        Vec3 randomized = _lensRadius * Vec3.RandomInUnitDisk();
        Vec3 offset = _u * randomized.X + _v * randomized.Y;
        return new Ray(
            _origin + offset,
            _lowerLeftCorner + s * _horizontal + t * _vertical - _origin - offset,
            _time0 + (_time1 - _time0) * Random.Shared.NextDouble()
            );
    }

    public double Time0 => _time0;
    public double Time1 => _time1;

    private Point3 _origin;
    private Point3 _lowerLeftCorner;
    private Vec3 _horizontal;
    private Vec3 _vertical;
    private Vec3 _u;
    private Vec3 _v;
    private Vec3 _w;
    private double _lensRadius;
    private double _time0;
    private double _time1;
}
