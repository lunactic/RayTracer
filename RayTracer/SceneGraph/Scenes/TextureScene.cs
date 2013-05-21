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

            //List of objects
            //Sphere sphere = new Sphere(textureMaterial, new Vector3(0, 0, 0), .3f);
            ColladaParser parser = new ColladaParser();
            parser.ParseColladaFile("./geometries/collada/desk.dae");

            Camera = parser.Cameras.Values.First();
            Camera.PreProcess();
            
            Film = new Film(Camera.ScreenWidth,Camera.ScreenHeight);

            Objects = new IntersectableList();
            
            foreach (IIntersectable intersectable in parser.Meshes.Values)
            {
                Objects.Add(intersectable);
            }

            Lights = new List<ILight>();
            foreach (ILight light in parser.Lights.Values)
            {
                Lights.Add(light);
            }
        }

    }
}
