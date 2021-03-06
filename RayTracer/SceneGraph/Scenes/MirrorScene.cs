﻿using System;
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
    public class MirrorScene : Scene
    {
        public MirrorScene()
        {
            FileName = "Assignment1_Mirror.jpg";
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
            Sphere sphere = new Sphere(new BlinnPhongMaterial(new Color(0.8f, 0, 0), new Color(0.6f, 0.6f, 0.6f), 30f), new Vector3(0f, 0f, 0f), 0.2f);
            //Sphere sphere = new Sphere(new LambertMaterial(new Color(0.8f, 0.8f, 0.8f)), new Vector3(0f, 0f, 0f), 0.2f);

            Sphere sphere2 = new Sphere(new MirrorMaterial(0.8f), new Vector3(0.4f, 0.2f, -0.3f), .3f);
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
            /*int[] i = { 0, 1, 2, 2, 3, 0 };
            Mesh rectangle = new Mesh(new LambertMaterial(new Color(0, 0, 1)), i,
                    -.25f, .999998f, .25f,
                    -.25f, .999999f, -.25f,
                     .25f, .999999f, -.25f,
                     .25f, .999998f, .25f);*/
            Rectangle rectangle = new Rectangle(new Vector3(-0.2f, .99f, 0f), new Vector3(.5f, 0, 0), new Vector3(0, 0, 0.5f))
          {
              Material = new LambertMaterial(new Color(0, 0, 1))
          };
            Objects = new IntersectableList();

            Objects.Add(rectangle);
            Objects.Add(sphere);
            Objects.Add(sphere2);
            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);

            Lights = new List<ILight>();
            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(0.7f, 0.7f, 0.7f));
            ILight light2 = new PointLight(new Vector3(-0.8f, 0.2f, 0.0f), new Color(.5f, .5f, .5f));
            ILight light3 = new AreaLight(new Color(0.1f, 0.1f, 0.1f), rectangle);
            Lights.Add(light3);
            //Lights.Add(light2);
            //Lights.Add(light);
        }
    }
}
