using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Helper;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Integrators;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.Tracer
{
    class RayTracerKdTree: AbstractRayTracer
    {

        public RayTracerKdTree()
        {
            FileName = "Assignment2_KDTree.jpg";
            scene = new Scene { BackgroundColor = Color.Black };
            integrator = new ShadowIntegrator(scene);
            camera = new Camera
            {
                FieldOfView = 55f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(0, 0, 10, 1),
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

            Mesh mesh = new Mesh() { Material = new LambertMaterial(Color.Red) };
            mesh.CreateMeshFromObjectFile("./geometries/bunny.obj", 1.0f);
            //Matrix4 m1 = Matrix4.Scale(0.5f);
            //Instance i = new Instance(mesh, m1);

            BspAccelerator accelerator = new BspAccelerator();
            mesh.BuildBoundingBox();
            accelerator.Construct(mesh);    

            scene.IntersectableList.Objects.Add(p1);
            scene.IntersectableList.Objects.Add(p2);
            scene.IntersectableList.Objects.Add(p3);
            scene.IntersectableList.Objects.Add(p4);
            scene.IntersectableList.Objects.Add(p5);
            //scene.IntersectableList.Objects.Add(mesh);
            scene.IntersectableList.Objects.Add(accelerator);
            ILight light = new PointLight(new Vector3(0.0f, 2.8f, 1.8f), new Color(.7f, .7f, .7f, 1f));
            ILight light2 = new PointLight(new Vector3(-0.8f, 1.2f, 2f), new Color(.5f, .5f, .5f, 1f));
            scene.Lights.Add(light);
            scene.Lights.Add(light2);
        }
    }
}
