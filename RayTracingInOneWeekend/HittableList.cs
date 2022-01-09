using RayTracingInOneWeekend.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingInOneWeekend;

internal class HittableList : IHittable
{
    public HittableList() { }

    public HittableList(IHittable obj)
    {
        _list.Add(obj);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public void Add(IHittable obj)
    {
        _list.Add(obj);
    }

    public HitRecord? Hit(in Ray r, double tMin, double tMax)
    {
        HitRecord? temp = null;
        var closestSoFar = tMax;
        foreach( IHittable obj in _list )
        {
            HitRecord? hit = obj.Hit(r, tMin, closestSoFar);
            if (hit != null)
            {
                temp = hit;
                closestSoFar = hit.T;
            }
        }
        return temp;
    }

    public Aabb? GetAabb(double time0, double time1)
    {
        if (_list.Count == 0) return null;

        Aabb? result = null;
        foreach( var obj in _list)
        {
            var box = obj.GetAabb(time0, time1);
            if (box == null) continue; // the book has "return null" which sounds wrong?
            result = result == null ? box : Aabb.SurroundingBox(result, box);
        }

        return result;
    }

    public BvhNode AsBvh(double time0, double time1) => new BvhNode(_list, time0, time1);

    private List<IHittable> _list = new();
}

