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
    public class SkyBoxScene : Scene
    {
        public SkyBoxScene()
        {
            FileName = "Assignment4_SkyBox.jpg";
            Camera.FieldOfViewX = 30f;
            Camera.FieldOfViewY = 30f;
            Camera.ScreenWidth = 512;
            Camera.ScreenHeight = 512;
            Camera.Eye = new Vector4(0, 0, -2, 1);
            Camera.Up = new Vector4(0, 1, 0, 1);
            Camera.LookAt = new Vector4(0, 0, 0, 1);

            Camera.PreProcess();
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            Texture top = new Texture("./Textures/SkyBox_farm/skybox_country_paths_top.jpg");
            Texture back = new Texture("./Textures/SkyBox_farm/skybox_country_paths_back.jpg");
            Texture front = new Texture("./Textures/SkyBox_farm/skybox_country_paths_front.jpg");
            Texture left = new Texture("./Textures/SkyBox_farm/skybox_country_paths_left.jpg");
            Texture right = new Texture("./Textures/SkyBox_farm/skybox_country_paths_right.jpg");
            Texture bottom = new Texture("./Textures/SkyBox_farm/skybox_country_paths_bottom.jpg");
            
            //SkyBox.LoadSkybox(left, right, top, bottom, front, back);

            PerlinNoise.Initialize();

            Sphere sphere = new Sphere(new LambertMaterial(new Color(1, 0, 0)), new Vector3(0, 0, 2f), 1f);
            sphere.Material.Noise = Noise.Bumb;
            //Sphere sphere = new Sphere(new MirrorMaterial(10f), new Vector3(0, 0, 2f), 1f);

            Objects = new IntersectableList();
            Lights = new List<ILight>();

            Objects.Add(sphere);
            ILight light = new DirectionalLight(new Vector3(0f, 0, -1f), new Color(1, 1, 1));
            Lights.Add(light);
            
        }
    }
}
