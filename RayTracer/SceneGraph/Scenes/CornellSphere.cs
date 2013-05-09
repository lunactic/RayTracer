using System;
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
    public class CornellSphere : Scene
    {
        public CornellSphere()
        {
            FileName = "CornellBox.jpg";

            Integrator = (IIntegrator) Activator.CreateInstance(Constants.Integrator);
            // Make camera and film
            Camera = new PinholeCamera()
                {
                    Eye = new Vector4(0, 0, 40f, 1f),
                    LookAt = new Vector4(0f, 0f, 9.9f, 1f),
                    Up = new Vector4(0f, 1f, 0f, 1f),
                    FieldOfViewX = 60f,
                    ScreenHeight = 512,
                    ScreenWidth = 512
                };
            Camera.PreProcess();
            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);
            // Colors
            LambertMaterial white = new LambertMaterial(new Color(0.65f, 0.65f, 0.65f));

            int[] i = {0,1,2,3,4,5};

    ;
            Mesh floor = new Mesh(white,i,
                         -10f,-10f, 10f,
                          10f,-10f,-10f,
                         -10f,-10f,-10f,
                          10f,-10f, 10f,
                          10f,-10f,-10f,
                         -10f,-10f, 10f);


            Rectangle rect = new Rectangle(new Vector3(-3.5f, 9.999999999f, -3.5f), new Vector3(0, 0,7), new Vector3(7, 0,0));
            // List of objects
            Objects = new IntersectableList();
            Objects.Add(rect);
            Sphere sphere1 = new Sphere(white,new Vector3(5,-6,1),4f  );
            Objects.Add(sphere1);
            Objects.Add(floor);
            // List of lights
            Lights = new List<ILight>();
            ILight light = new AreaLight(new Color(13, 13, 13), rect);
            Lights.Add(light);
        }
    }
}
