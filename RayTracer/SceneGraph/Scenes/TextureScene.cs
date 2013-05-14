using RayTracer.Collada;
using RayTracer.Helper;
using RayTracer.SceneGraph.Accelerate;
using RayTracer.SceneGraph.Cameras;
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
    public class TextureScene : Scene
    {
        public TextureScene()
        {
            FileName = "Assignment4_Texture.jpg";
            //scene = new Scene { BackgroundColor = Color.Black };
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

            Camera.FieldOfViewX = 30f;
            Camera.FieldOfViewY = 30f;
            Camera.ScreenWidth = 100;
            Camera.ScreenHeight = 100;
            Camera.Eye = new Vector4(0, 0, -2, 1);
            Camera.Up = new Vector4(0, 1, 0, 1);
            Camera.LookAt = new Vector4(0, 0, 0, 1);

            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            LambertMaterial green = new LambertMaterial(new Color(0, 1, 0));
            LambertMaterial red = new LambertMaterial(new Color(1, 0, 0));
            LambertMaterial blue = new LambertMaterial(new Color(0, 0, 1));
            LambertMaterial gray = new LambertMaterial(new Color(0.8f, 0.8f, 0.8f));
            LambertMaterial black = new LambertMaterial(new Color(0, 0, 0));
            LambertMaterial white = new LambertMaterial(new Color(1, 1, 1));

            LambertMaterial textureMaterial = new LambertMaterial(new Color(1, 1, 1));
            Texture tex = new Texture("./Textures/star.jpg");
            textureMaterial.Texture = tex;

            //List of objects
            //Sphere sphere = new Sphere(textureMaterial, new Vector3(0, 0, 0), .3f);
            ColladaParser parser = new ColladaParser();
            parser.ParseColladaFile("./geometries/collada/test_Pyramids.dae");
            Objects = new IntersectableList();

            foreach (Mesh intersectable in parser.Meshes.Values)
            {
                Material mat = new LambertMaterial(new Color(1, 0, 0));
                intersectable.Material = mat;
                //intersectable.Material = textureMaterial;
                Matrix4 m = Matrix4.Scale(0.15f);
                Matrix4 rotX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(75f));
                Matrix4 rotY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(10f));

                Matrix4 rot = Matrix4.Mult(rotX, rotY);

                Matrix4 res = Matrix4.Mult(m, rot);

                BspAccelerator acc = new BspAccelerator();
                //acc.Construct(intersectable);
                Instance i = new Instance(intersectable,res);
           
                Objects.Add(intersectable);
            }
            //Matrix4 mat2 = Matrix4.CreateRotationX((float)(MathHelper.DegreesToRadians(45f)));

            //Instance inst = new Instance(sphere, mat2);

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
            //Objects.Add(rect);
            //Objects.Add(sphere);
            //Objects.Add(m);
            Objects.Add(p1);
            Objects.Add(p2);
            Objects.Add(p3);
            Objects.Add(p4);
            Objects.Add(p5);

            Lights = new List<ILight>();
            ILight light = new PointLight(new Vector3(0.0f, 0.8f, 0.5f), new Color(1, 1, 1));
            //Lights.Add(light);
            light = new PointLight(new Vector3(0f, 0, -0.9f), new Color(1, 1, 1));
            Lights.Add(light);
        }

    }
}
