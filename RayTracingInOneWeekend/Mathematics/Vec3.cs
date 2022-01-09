using System.Runtime.CompilerServices;

namespace RayTracingInOneWeekend.Mathematics;

// Yeah I suppose we could just use Vector3 from Numerics, but a) doubles b) let's keep the code as close to the book as reasonable
internal readonly struct Vec3
{
    public Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public Vec3(in Vec3 other)
    {
        X = other.X;
        Y = other.Y;
        Z = other.Z;
    }

    public Vec3(double e0, double e1, double e2)
    {
        X = e0;
        Y = e1;
        Z = e2;
    }

    public Vec3(double e)
    {
        X = e;
        Y = e;
        Z = e;
    }

    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    public double this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ElementAt(this, i);
        //set => ElementAt(this, i) = value;
    }

    public double Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public double LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3 UnitVector() => this / Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool isNearZero()
    {
        const double epsilon = 1e-8;
        return Math.Abs(X) < epsilon && Math.Abs(Y) < epsilon && Math.Abs(Z) < epsilon;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(in Vec3 other) => new Vec3(-other.X, -other.Y, -other.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator +(in Vec3 left, in Vec3 right) => new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator -(in Vec3 left, in Vec3 right) => new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec3 left, double t) => new Vec3(left.X * t, left.Y * t, left.Z * t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(double t, in Vec3 right) => right * t;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator /(in Vec3 left, double t) => left * (1/t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 operator *(in Vec3 left, in Vec3 right) => new Vec3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(in Vec3 left, in Vec3 right) => left.X * right.X + left.Y * right.Y + left.Z * right.Z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Cross(in Vec3 left, in Vec3 right) => new Vec3(
            left.Y * right.Z - left.Z * right.Y,
            left.Z * right.X - left.X * right.Z,
            left.X * right.Y - left.Y * right.X
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Min(in Vec3 left, in Vec3 right) => new Vec3(
            Math.Min(left.X, right.X),
            Math.Min(left.Y, right.Y),
            Math.Min(left.Z, right.Z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3 Max(in Vec3 left, in Vec3 right) => new Vec3(
            Math.Max(left.X, right.X),
            Math.Max(left.Y, right.Y),
            Math.Max(left.Z, right.Z));

    public static Vec3 Reflect(in Vec3 vector, in Vec3 normal) => vector - 2 * Dot(vector, normal)  * normal;

    public static Vec3 Refract(in Vec3 uv, in Vec3 normal, double etaiOverEtat) {
        var cosTheta = Math.Min(-Dot(uv, normal), 1.0);
        Vec3 rOutPerp = etaiOverEtat * (uv + cosTheta * normal);
        Vec3 rOutParallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared ) ) * normal;
        return rOutPerp + rOutParallel;
    }


    public override string ToString()
    {
        return $"{X} {Y} {Z}";
    }

    public string AsColorString( int samplesPerPixel = 1 )
    {
        // Divide the color by the number of samples and gamma-correct for gamma=2.0.
        var scale = 1.0 / samplesPerPixel;
        var r = Math.Sqrt(X * scale);
        var g = Math.Sqrt(Y * scale);
        var b = Math.Sqrt(Z * scale);
        var ir = (int)(256 * Math.Clamp(r, 0.0, 0.999));
        var ig = (int)(256 * Math.Clamp(g, 0.0, 0.999));
        var ib = (int)(256 * Math.Clamp(b, 0.0, 0.999));
        return $"{ir} {ig} {ib}";
    }

    public static Vec3 Random()
    {
        return new Vec3(System.Random.Shared.NextDouble(), System.Random.Shared.NextDouble(), System.Random.Shared.NextDouble());
    }

    public static Vec3 Random(double min, double max)
    {
        var scale = (max - min);
        return new Vec3(
            min + scale * System.Random.Shared.NextDouble(),
            min + scale * System.Random.Shared.NextDouble(),
            min + scale * System.Random.Shared.NextDouble()
        );
    }

    public static Vec3 RandomInUnitSphere()
    {
        while(true)
        {
            var p = Random(-1, 1);
            if (p.LengthSquared >= 1) continue;
            return p;
        }
    }

    public static Vec3 RandomUnitVector() => RandomInUnitSphere().UnitVector();

    public static Vec3 RandomInUnitDisk()
    {
        while(true)
        {
            var p = new Vec3(-1.0 + 2.0 * System.Random.Shared.NextDouble(), -1.0 * 2.0 * System.Random.Shared.NextDouble(), 0);
            if ( p.LengthSquared >= 1) continue;
            return p;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref double ElementAt(in Vec3 src, nint idx)
    {
        return ref Unsafe.Add(ref Unsafe.As<Vec3, double>(ref Unsafe.AsRef(src)), idx);
    }

}

