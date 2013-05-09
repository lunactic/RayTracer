using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Scenes
{
    public class RefractionScene : Scene
    {
        public RefractionScene()
        {
            FileName = "Assignment1_Refraction.jpg";
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);
            Camera = new PinholeCamera()
            {
                FieldOfViewX = 30f,
                FieldOfViewY = 30f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            //List of objects
            Sphere sphere1 = new Sphere(new RefractiveMaterial(1.5f), new Vector3(0f, 0f, 0f), 0.4f);
            Sphere sphere2 = new Sphere(new BlinnPhongMaterial(new Color(0.8f, 0f, 0f), new Color(0.8f, 0.8f, 0.8f), 30f), new Vector3(0.4f, 0.2f, -0.3f), 0.3f);
            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
            {
                Name = "P1",
                Material = new LambertMaterial(new Color(0f, 0.8f, 0.8f))

            };
            Plane p2 = new Plane(1f, new Vector3(0, 0, 1))
            {
                Name = "P2",
                Material = new LambertMaterial(new Color(0.3f, 0.8f, 0.8f))
            };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
            {
                Name = "P3",
                Material = new LambertMaterial(new Color(1.0f, 0.8f, 0.8f))
            };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
            {
                Name = "P4",
                Material = new LambertMaterial(new Color(0f, 0.8f, 0f))
            };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
            {
                Name = "P5",
                Material = new LambertMaterial(new Color(0.8f, 0.8f, 0.8f))
            };

            Objects = new IntersectableList();

            Objects.Add(sphere1);
            Objects.Add(sphere2);
            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);

            Lights = new List<ILight>();

            ILight light1 = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(0.7f, 0.7f, 0.7f));
            ILight light2 = new PointLight(new Vector3(-0.8f, 0.2f, 0f), new Color(0.5f, 0.5f, 0.5f));
            Lights.Add(light1);
            Lights.Add(light2);    

        }
    }
}
