using RayTracer.SceneGraph;
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

namespace RayTracer.Tracer
{
    public class RayTracerInstancing : AbstractRayTracer
    {

        public RayTracerInstancing()
        {
            NumberOfThreads = 8;
            FileName = "Assignment1_Instancing.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            integrator = new ShadowIntegrator();
            camera = new Camera
            {
                FieldOfView = 60f,
                ScreenWidth = 256,
                ScreenHeight = 256,
                Eye = new Vector4(0, 0, 2, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(0, 0, 0, 1)
            };
            camera.PreProcess();

            film = new Film(camera.ScreenWidth, camera.ScreenHeight);

            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
            {
                Name = "P1",
                Material = new LambertMaterial(new Color(0f, 0.8f, 0.8f, 1.0f))

            };
            Plane p2 = new Plane(1f, new Vector3(0, 0, 1))
            {
                Name = "P2",
                Material = new LambertMaterial(new Color(0.3f, 0.8f, 0.8f, 1.0f))
            };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
            {
                Name = "P3",
                Material = new LambertMaterial(new Color(1.0f, 0.8f, 0.8f, 1.0f))
            };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
            {
                Name = "P4",
                Material = new LambertMaterial(new Color(0f, 0.8f, 0f, 1.0f))
            };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
            {
                Name = "P5",
                Material = new LambertMaterial(new Color(0.8f, 0.8f, 0.8f, 1.0f))
            };

            Mesh mesh = new Mesh() { Material = new LambertMaterial(new Color(0.5f, 0.5f, 0.5f, 1.0f)) };
            mesh.CreateMeshFromObjectFile("./geometries/teapot.obj", 1.0f);

            Matrix4 m1 = Matrix4.Scale(0.5f);
            m1.Translation = new Vector4(0, -0.25f, 0, 1);


            Matrix4 m2 = Matrix4.Scale(0.5f);
            m2.Translation = new Vector4(0, 0.25f, 0, 1);



            Instance i1 = new Instance(mesh, m1);
            Instance i2 = new Instance(mesh, m2);


            scene.IntersectableList.Objects.Add(p1);
            scene.IntersectableList.Objects.Add(p2);
            scene.IntersectableList.Objects.Add(p3);
            scene.IntersectableList.Objects.Add(p4);
            scene.IntersectableList.Objects.Add(p5);
            scene.IntersectableList.Objects.Add(i1);
            scene.IntersectableList.Objects.Add(i2);

            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(.7f, .7f, .7f, 1f));
            ILight light2 = new PointLight(new Vector3(-0.8f, 0.2f, 1f), new Color(.5f, .5f, .5f, 1f));
            scene.Lights.Add(light);
            scene.Lights.Add(light2);
        }

    }
}
