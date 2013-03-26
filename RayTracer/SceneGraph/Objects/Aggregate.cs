using RayTracer.SceneGraph.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Objects
{
    public abstract class Aggregate
    {
        public abstract List<IIntersectable> GetObjects();
    }
}
