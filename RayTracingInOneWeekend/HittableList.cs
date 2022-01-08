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

    private List<IHittable> _list = new();
}

