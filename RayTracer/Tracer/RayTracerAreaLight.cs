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
using RayTracer.SceneGraph.Scenes;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.Tracer
{
    public class RayTracerAreaLight : AbstractRayTracer
    {
        public RayTracerAreaLight()
        {
            NumberOfThreads = 4;
            FileName = "Assignment2_AreaLight.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            integrator = new RefractionIntegrator();
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

            film = new Film(camera.ScreenWidth, camera.ScreenHeight);

            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
            {
                Name = "P1",
                Material = new LambertMaterial(new Color(0.159f, 1.957f, 1.016f, 1f))

            };
            Plane p2 = new Plane(1f, new Vector3(0, 0, 1))
            {
                Name = "P2",
                Material = new LambertMaterial(new Color(0.159f, 1.957f, 1.016f, 1.0f))
            };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
            {
                Name = "P3",
                Material = new LambertMaterial(new Color(0.159f, 1.957f, 1.016f, 1.0f))
            };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
            {
                Name = "P4",
                Material = new LambertMaterial(new Color(0.159f, 1.957f, 1.016f, 1.0f))
            };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
            {
                Name = "P5",
                Material = new LambertMaterial(new Color(0.159f, 1.957f, 1.016f, 1.0f))
            };

            Sphere sphere = new Sphere(new LambertMaterial(new Color(.3f, .3f, .3f, 1f)), new Vector3(0, 0, 0), .3f);

            Rectangle rectangle = new Rectangle(new Vector3(-.2f, 1, 0), new Vector3(.5f, 0, 0), new Vector3(0, 0f, .5f))
            {
                Material = new LambertMaterial(new Color(0f,0f,1f,1f))
            };
            scene.IntersectableList.Objects.Add(rectangle);
            scene.IntersectableList.Objects.Add(sphere);
            scene.IntersectableList.Objects.Add(p1);
            scene.IntersectableList.Objects.Add(p2);
            scene.IntersectableList.Objects.Add(p3);
            scene.IntersectableList.Objects.Add(p4);
            scene.IntersectableList.Objects.Add(p5);
            ILight light = new PointLight(new Vector3(0f, .2f, 0.8f), new Color(.7f, .7f, .7f,1f));
            scene.Lights.Add(light);
            light = new DirectionalLight(new Vector3(0, 1f, 1), new Color(.2f, .2f, .2f,1f));
            scene.Lights.Add(light);
            light = new AreaLight(new Color(10, 10, 10,1f), rectangle);
            scene.Lights.Add(light);
        }
    }
}
