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
    class RayTracerKdTree : AbstractRayTracer
    {

        public RayTracerKdTree()
        {
            
            NumberOfThreads = 8;
            FileName = "Assignment2_KdTree.jpg";
            scene = new Scene { BackgroundColor = Color.Black };
            integrator = new RefractionIntegrator(scene);
            camera = new Camera
            {
                FieldOfView = 60f,
                ScreenWidth = 256,
                ScreenHeight = 256,
                Eye = new Vector4(1, 0.5f, 1.8f, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(-0.4f, -.2f, 0, 1)
            };
            camera.PreProcess();

            film = new Film(camera.ScreenWidth, camera.ScreenHeight);

            Plane p1 = new Plane(1f, new Vector3(0, 1, 0))
            {
                Name = "P1",
                Material = new LambertMaterial(new Color(0.059f, 0.957f, 0.016f, 1.0f))

            };
            Plane p2 = new Plane(1f, new Vector3(0, 0, 1))
            {
                Name = "P2",
                Material = new LambertMaterial(new Color(0f, 0f, 1f, 1.0f))
            };
            Plane p3 = new Plane(1f, new Vector3(-1, 0, 0))
            {
                Name = "P3",
                Material = new LambertMaterial(new Color(0.059f, 0.957f, 0.016f, 1.0f))
            };
            Plane p4 = new Plane(1f, new Vector3(1, 0, 0))
            {
                Name = "P4",
                Material = new MirrorMaterial(new Color(0.8f, 0.8f, 0.8f, 1.0f))
            };
            Plane p5 = new Plane(1f, new Vector3(0, -1, 0))
            {
                Name = "P5",
                Material = new LambertMaterial(new Color(0.059f, 0.957f, 0.016f, 1.0f))
            };

            Mesh mesh = new Mesh { Material = new BlinnPhongMaterial(new Color(0.753f, 0.753f, 0.753f, 1.0f),new Color(1,1,1,1), 30) };
            mesh.CreateMeshFromObjectFile("./geometries/dragon.obj", 1.0f);
            
            BspAccelerator tree = new BspAccelerator();
            tree.Construct(mesh);

            Matrix4 m1 = Matrix4.Scale(0.5f);
            Matrix4 rotation = Matrix4.CreateRotationX((float)-Math.PI/2);
            m1 = m1*rotation;
            rotation = Matrix4.CreateRotationZ((float) -Math.PI/4);
            m1 = m1*rotation;
            m1.Translation = new Vector4(0.2f,-0.65f,-0.2f,1f);
            Instance i1 = new Instance(tree,m1);


            scene.IntersectableList.Objects.Add(p1);
            scene.IntersectableList.Objects.Add(p2);
            scene.IntersectableList.Objects.Add(p3);
            scene.IntersectableList.Objects.Add(p4);
            scene.IntersectableList.Objects.Add(p5);
            scene.IntersectableList.Objects.Add(i1);
            //scene.IntersectableList.Objects.Add(mesh);
            ILight light = new PointLight(new Vector3(0.0f, 0.2f, 0.8f), new Color(.7f, .7f, .7f, 1f));
            ILight light2 = new PointLight(new Vector3(.3f, 0.3f, 0f), new Color(.2f, .2f, .2f, 1f));
            scene.Lights.Add(light);
            scene.Lights.Add(light2);
        }
    }
}
