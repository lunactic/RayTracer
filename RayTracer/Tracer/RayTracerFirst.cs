using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.Tracer
{
    public class RayTracerFirst : AbstractRayTracer
    {

        public RayTracerFirst()
        {
            FileName = "Assignment1_First.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            integrator = new BinaryIntegrator();
            camera = new Camera
            {
                FieldOfView = 60.0f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            camera.PreProcess();

            Plane p = new Plane(1.0f,new Vector3(0,1,0));
            scene.IntersectableList.Objects.Add(p);
            film = new Film(camera.ScreenWidth, camera.ScreenHeight);
            integrator = new BinaryIntegrator();


        }
    }
}
