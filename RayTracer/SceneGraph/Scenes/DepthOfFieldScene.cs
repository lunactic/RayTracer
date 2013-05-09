using RayTracer.Helper;
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
    public class DepthOfFieldScene : Scene
    {
        public DepthOfFieldScene()
        {
            FileName = "Assignment4_DoF.jpg";
       
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

            Camera.Eye = new Vector4(-5f, 0f, -2.8f, 1f);
            Camera.LookAt = new Vector4(0f, 0f, -2.8f, 1f);
            Camera.Up = new Vector4(0, 1, 0, 1);
            Camera.Apperture = 0.25f;
            Camera.D = 7.5f;
            Camera.FieldOfViewX = 50f;
            Camera.FieldOfViewY = 50f;
            Camera.ScreenWidth = 512;
            Camera.ScreenHeight = 512;
            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);

            Camera.PreProcess();

            Objects = new IntersectableList();
            /*   
            shapes[0].type = SHAPE_SPHERE;
            VectorSet(shapes[0].sphere.center, -2.6f, -0.5f, -1.5f);
	        shapes[0].sphere.radius = 1;
	        VectorSet(shapes[0].move_velocity, 0, 0, 0);
	        VectorSet(shapes[0].color, 1, 0, 0);
            */
            Sphere sphere = new Sphere(new LambertMaterial(new Color(1, 0, 0)),new Vector3(-2.6f,-0.5f,-1.5f),1f);
            Objects.Add(sphere);
            /*
	        shapes[1].type = SHAPE_SPHERE;
	        VectorSet(shapes[1].sphere.center, 2.6f, 0.5f, -4);
	        shapes[1].sphere.radius = 1;
	        VectorSet(shapes[1].move_velocity, 0, 0, 0);
	        VectorSet(shapes[1].color, 0, 1, 0);
            */
            sphere = new Sphere(new LambertMaterial(new Color(1, 0, 0)), new Vector3(-2.6f, -0.5f, -1.5f), 1f);
            Objects.Add(sphere);
            /*
	        shapes[3].type = SHAPE_TRI;
	        VectorSet(shapes[2].tri.verts[2], 0, 0.2f, -5);
	        VectorSet(shapes[2].tri.verts[1], 0, -0.4f, -1);
	        VectorSet(shapes[2].tri.verts[0], 0, 0.6f, -2);
	        VectorSet(shapes[2].move_velocity, 0, 0, 0);
	        VectorSet(shapes[2].color, 1, 1, 0);
            */
            /*
	        VectorSet(lights[0].ambient, 0.1f, 0.1f, 0.1f);
	        VectorSet(lights[0].diffuse, 1, 1, 1);
	        VectorSet(lights[0].specular, 0.1f, 0.1f, 0.1f);
	        VectorSet(lights[0].pos, -2, 2, -2);
             */
            ILight light = new PointLight(new Vector3(-2f, 2f, -2f), new Color(1, 1, 1));
            Lights = new List<ILight>();
            Lights.Add(light);
         
        }
    }
}
