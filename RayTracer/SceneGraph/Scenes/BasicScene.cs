using RayTracer.Helper;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Scenes
{
    public class BasicScene : Scene
    {
        public BasicScene()
        {
            FileName = "Assignment1_Basic.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);
            Camera = new Camera
            {
                FieldOfView = 60f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, -2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            LambertMaterial green = new LambertMaterial(new Color(0,1,0));
            LambertMaterial red = new LambertMaterial(new Color(1,0,0));
            LambertMaterial blue = new LambertMaterial(new Color(0,0,1));
            LambertMaterial gray = new LambertMaterial(new Color(0.8f,0.8f,0.8f));
            LambertMaterial black = new LambertMaterial(new Color(0,0,0));
            LambertMaterial white = new LambertMaterial(new Color(1,1,1));

            //List of objects
            Sphere sphere = new Sphere(white, new Vector3(0, 0, .25f), .3f);


            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
            {
                Material = white
            };
            Plane p2 = new Plane(1f, new Vector3(0, 0, -1))
            {
                Material = white
            };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
            {
                Material = green
            };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
            {
                Material = blue
            };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
            {
                Material = white
            };
            Rectangle rect = new Rectangle(new Vector3(-0.2f, .99f, 0f), new Vector3(.5f, 0, 0), new Vector3(0, 0, 0.5f))
            {
                Material = blue
            };
            Objects = new IntersectableList();
            Objects.Add(rect);
            Objects.Add(sphere);
            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);

            Lights = new List<ILight>();

            //ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(1,1,1));
            ILight light = new AreaLight(new Color(10, 10, 10), rect);
     
            Lights.Add(light);

        }
    }
}
