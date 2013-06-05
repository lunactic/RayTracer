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
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

            Texture back = new Texture("./Textures/Skybox_stars/backmo.jpg",false);
            Texture bottom = new Texture("./Textures/Skybox_stars/botmo.jpg", false);
            Texture front = new Texture("./Textures/Skybox_stars/frontmo.jpg", false);
            Texture left = new Texture("./Textures/Skybox_stars/leftmo.jpg", false);
            Texture right = new Texture("./Textures/Skybox_stars/rightmo.jpg", false);
            Texture top = new Texture("./Textures/Skybox_stars/topmo.jpg", false);
            
            SkyBox.LoadSkybox(left,right,top,bottom,front,back);

            //List of objects
            //Sphere sphere = new Sphere(textureMaterial, new Vector3(0, 0, 0), .3f);
            ColladaParser parser = new ColladaParser();
            parser.ParseColladaFile("./geometries/collada/deathstar.dae",1024);

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
