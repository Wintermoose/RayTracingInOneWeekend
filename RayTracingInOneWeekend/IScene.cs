using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingInOneWeekend;

internal interface IScene
{
    HittableList GetWorld();

    Camera GetCamera();

    (double aspectRatio, int samplesPerPixel, int maxDepth) GetPreferredParameters();
}

