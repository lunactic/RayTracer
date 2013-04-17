using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph;
using RayTracer.Structs;
using RayTracer.SceneGraph.Objects;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Light;

namespace RayTracer.Tracer
{
    public class RayTracer
    {

        public RayTracer()
        {

        }

        static void Main()
        {
            IRayTracer rayTracer = null;
            const int sceneSelector = 3;

            switch (sceneSelector)
            {
                case 0: rayTracer = new RayTracerFirst();
                    break;
                case 1: rayTracer = new RayTracerBasic();
                    break;
                case 2: rayTracer = new RayTracerInstancing();
                    break;
                case 3: rayTracer = new RayTracerMirror();
                    break;
                case 4: rayTracer = new RayTracerKdTree();
                    break;
                case 5: rayTracer = new RayTracerSphereTest();
                    break;
                case 6: rayTracer = new RayTracerRefraction();
                    break;
                case 7: rayTracer = new RayTracerAreaLight();
                    break;
            }

            //Create the RayTracer setup you want to use here
            if (rayTracer != null) rayTracer.Render();
        }
    }
}
