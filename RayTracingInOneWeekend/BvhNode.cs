using Point3 = RayTracingInOneWeekend.Mathematics.Vec3;
using Ray = RayTracingInOneWeekend.Mathematics.Ray;

namespace RayTracingInOneWeekend;

internal class BvhNode : IHittable
{
    public BvhNode( List<IHittable> list, double time0, double time1 )
        : this(list.ToArray().AsSpan(), list.Select( obj => obj.GetAabb(time0, time1)).ToArray().AsSpan(), time0, time1 )
    {
        
    }

    public BvhNode( Span<IHittable> src, Span<Aabb?> abbs, double time0, double time1 )
    {
        int axis = System.Random.Shared.Next(0, 3);

        var comparer = (Aabb? box0, Aabb? box1) =>
        {
            if (box0 == null || box1 == null) throw new Exception("Missing AABB");
            var v0 = box0.Min[axis];
            var v1 = box1.Min[axis];
            return v0 < v1 ? -1 :
                v0 > v1 ? 1 : 0;            
        };

        int count = src.Length;

        if (count == 1)
        {
            _left = _right = src[0];
        }
        else if (count == 2)
        {
            if (comparer(abbs[0], abbs[1]) <= 0 )
            {
                _left = src[0];
                _right = src[1];
            } else
            {
                _left = src[1];
                _right = src[0];
            }
        }
        else
        {
            abbs.Sort(src, comparer.Invoke);
            var mid = count / 2;
            if ( mid == 1 )
            {
                _left = src[0];
            } else
            {
                _left = new BvhNode(src.Slice(0, mid), abbs.Slice(0, mid), time0, time1);
            }
            _right = new BvhNode(src.Slice(mid), abbs.Slice(mid), time0, time1);
        }

        var leftBox = _left.GetAabb(time0, time1);
        var rightBox = _right.GetAabb(time0, time1);
        if (leftBox == null || rightBox == null) throw new Exception("Bounding box not found");

        _box = Aabb.SurroundingBox(leftBox, rightBox);
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        return _box;
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        if (!_box.Hit(r, tMin, tMax)) return null;

        HitRecord? leftHit = _left.Hit(r, tMin, tMax);
        HitRecord? rightHit = _right.Hit(r, tMin, leftHit?.T ?? tMax);

        return rightHit ?? leftHit;
    }

    private IHittable _left;
    private IHittable _right;
    private Aabb _box;
}
