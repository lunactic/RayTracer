using System;
using System.Collections.Generic;
using System.Diagnostics;
using RayTracer.Collada;
using RayTracer.Helper;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Scenes
{
    public class ColladaScene : Scene
    {
        public ColladaScene()
        {
            FileName = "Assignment_Collada.jpg";
            ColladaParser parser = new ColladaParser();
            parser.ParseColladaFile("./geometries/collada/heart.dae");
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);
            /*Camera = new PinholeCamera()
            {
                FieldOfViewX = 60f,
                FieldOfViewY = 60f,
                ScreenWidth = 128,
                ScreenHeight = 128,
                Eye = new Vector4(0, 0, -50, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            Camera.PreProcess();
            */

            LambertMaterial green = new LambertMaterial(new Color(0, 1, 0));
            LambertMaterial red = new LambertMaterial(new Color(1, 0, 0));
            LambertMaterial blue = new LambertMaterial(new Color(0, 0, 1));
            LambertMaterial gray = new LambertMaterial(new Color(0.8f, 0.8f, 0.8f));
            LambertMaterial black = new LambertMaterial(new Color(0, 0, 0));
            LambertMaterial white = new LambertMaterial(new Color(1, 1, 1));

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

            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);

            Lights = new List<ILight>();

            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(1, 1, 1));
            Lights.Add(light);
            foreach (IIntersectable intersectable in parser.Meshes.Values)
            {
                Matrix4 m = Matrix4.Scale(0.1f);
                Matrix4 rot = Matrix4.CreateRotationX(45f);

                Matrix4 res = Matrix4.Mult(m, rot);

                Instance i = new Instance(intersectable, res);
                Objects.Add(i);
            }

            

            foreach (ICamera camera in parser.Cameras.Values)
            {
                Camera = camera;
            }

            if (Camera == null || float.IsNaN(Camera.AspectRation))
            {
                Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

                Camera.FieldOfViewX = 30f;
                Camera.FieldOfViewY = 30f;
                Camera.ScreenWidth = 512;
                Camera.ScreenHeight = 512;
                Camera.Eye = new Vector4(0, 0, -2, 1);
                Camera.Up = new Vector4(0, 1, 0, 1);
                Camera.LookAt = new Vector4(0, 0, 0, 1);

                Camera.PreProcess();
            }

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

        }
    }
}
