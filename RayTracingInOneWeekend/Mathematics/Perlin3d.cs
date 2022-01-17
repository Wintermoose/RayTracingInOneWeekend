using System;
using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;

namespace RayTracingInOneWeekend.Mathematics;
internal class Perlin3d
{
    const int POINT_COUNT = 256;

    public Perlin3d()
    {
        _randoms = new Vec3[POINT_COUNT];
        for (int i = 0; i < POINT_COUNT; i++)
        {
            _randoms[i] = Vec3.Random(-1, 1).UnitVector();
        }

        _permX = GeneratePermutations();
        _permY = GeneratePermutations();
        _permZ = GeneratePermutations();
    }

    public double Noise(in Point3 location)
    {
        double u = location.X - Math.Floor(location.X);
        double v = location.Y - Math.Floor(location.Y);
        double w = location.Z - Math.Floor(location.Z);

        int i = (int)Math.Floor(location.X);
        int j = (int)Math.Floor(location.Y);
        int k = (int)Math.Floor(location.Z);
        var c = new Vec3[2,2,2];
        for ( int di = 0; di < 2; ++di )
        {
            for (int dj = 0; dj < 2; ++dj )
            {
                for ( int dk = 0; dk < 2; ++dk )
                {
                    c[di, dj, dk] = _randoms[
                            _permX[(i + di) & 255] ^
                            _permY[(j + dj) & 255] ^
                            _permZ[(k + dk) & 255]
                        ];
                }
            }
        }
        // TODO: inline calculation above
        return PerlinInterpolation(c, u, v, w);
    }

    public double Turbulence(in Point3 location, int depth = 7) {
        var accum = 0.0;
        var loc = location;
        var weight = 1.0;

        for (int i = 0; i<depth; i++) {
            accum += weight * Noise(loc);
            weight *= 0.5;
            loc *= 2;
        }

        return Math.Abs(accum);
    }

    private static int[] GeneratePermutations()
    {
        int[] result = new int[POINT_COUNT];
        for (int i = 0; i < POINT_COUNT;i++) result[i] = i;
        Permutate(result);
        return result;
    }

    private static void Permutate(int[] array)
    {
        for ( int i = array.Length - 1; i > 0; --i )
        {
            int target = Random.Shared.Next(i);
            int tmp = array[i];
            array[i] = array[target];
            array[target] = tmp;
        }
    }

    private static double PerlinInterpolation(Vec3[,,] c, double u, double v, double w)
    {
        double uu = u * u * (3 - 2 * u);
        double vv = v * v * (3 - 2 * v);
        double ww = w * w * (3 - 2 * w);
        double accum = 0.0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Vec3 weight = new (u-i, v-j, w-k);
                    accum += (i * uu + (1 - i) * (1 - uu)) *
                            (j * vv + (1 - j) * (1 - vv)) *
                            (k * ww + (1 - k) * (1 - ww)) * Vec3.Dot(c[i,j,k], weight);
                }
            }
        }

        return accum;
    }

    private Vec3[] _randoms;
    private int[] _permX;
    private int[] _permY;
    private int[] _permZ;
}
