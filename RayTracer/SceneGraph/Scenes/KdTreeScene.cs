using RayTracer.Helper;
using RayTracer.SceneGraph.Accelerate;
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
    public class KdTreeScene : Scene
    {
        public KdTreeScene()
        {
            FileName = "Assignment2_KdTree.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);



            Camera.FieldOfViewX = 60f;
            Camera.FieldOfViewY = 60f;
            Camera.ScreenWidth = 512;
            Camera.ScreenHeight = 512;
            Camera.Eye = new Vector4(0, 0, 2, 1);
            Camera.Up = new Vector4(0, 1, 0, 1);
            Camera.LookAt = new Vector4(0, 0, 0, 1);
            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

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

            Mesh mesh = new Mesh() { Material = new LambertMaterial(new Color(0.5f, 0.5f, 0.5f)) };
            mesh.CreateMeshFromObjectFile("./geometries/buddha.obj", 1.0f);

            BspAccelerator tree = new BspAccelerator();
            tree.Construct(mesh);

            //Matrix4 m1 = Matrix4.Scale(0.5f);
            //m1.Translation = new Vector4(0, -0.25f, 0, 1);


            Matrix4 m2 = Matrix4.Scale(0.5f);
            m2.Translation = new Vector4(0, 0.25f, 0, 1);



            Instance i1 = new Instance(tree, Matrix4.Identity);
            //Instance i2 = new Instance(tree, m2);

            /*
            Instance i1 = new Instance(mesh, m1);
            Instance i2 = new Instance(mesh, m2);
            */
            Objects = new IntersectableList();

            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);
            Objects.Add(i1);
            //scene.IntersectableList.Objects.Add(i2);

            Lights = new List<ILight>();

            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.8f), new Color(.7f, .7f, .7f));
            ILight light2 = new PointLight(new Vector3(-0.8f, 0.2f, 1f), new Color(.5f, .5f, .5f));
            Lights.Add(light);
            Lights.Add(light2);
        }
    }
}
