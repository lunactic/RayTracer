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
            parser.ParseColladaFile("./geometries/collada/model.dae");
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);
            Camera = new PinholeCamera()
            {
                FieldOfView = 60f,
                ScreenWidth = 128,
                ScreenHeight = 128,
                Eye = new Vector4(0, 0, -50, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            Objects = new IntersectableList();

            foreach (Mesh mesh in parser.Meshes)
            {
                Objects.Add(mesh);
            }

            Lights = new List<ILight>();
            ILight light = new PointLight(new Vector3(0.0f, 10f, 10f), new Color(1,1,1));
            Lights.Add(light);
        }
    }
}
