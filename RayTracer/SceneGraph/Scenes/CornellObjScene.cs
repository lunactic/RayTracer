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
    public class CornellObjScene : Scene
    {
        public CornellObjScene()
        {
            FileName = "Assignment3_CornellBox.jpg";
            Integrator = (IIntegrator)Activator.CreateInstance(Constants.Integrator);

            Camera = new PinholeCamera()
            {
                FieldOfViewX = 39.3077f,
                FieldOfViewY = 39.3077f,
                ScreenWidth = 512,
                ScreenHeight = 512,
                Eye = new Vector4(278, 273, -800, 1),
                Up = new Vector4(0, 1, 0, 1),
                LookAt = new Vector4(278, 273, -799, 1)
            };
            Camera.PreProcess();

            Film = new Film(Camera.ScreenWidth, Camera.ScreenHeight);
        
            Mesh m1 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m1.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_0.obj", 1.0f);

            Mesh lightMesh = new Mesh() { Material = new LambertMaterial(new Color(0.78f, 0.78f, 0.87f)) };
            lightMesh.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_1.obj", 1.0f);

            //Matrix4 transfom = Matrix4.CreateTranslation(new Vector3(0f, -0.5f, 0f));

            Mesh m2 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m2.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_2.obj", 1.0f);


            Mesh m3 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m3.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_3.obj", 1.0f);


            Mesh m4 = new Mesh() { Material = new LambertMaterial(new Color(0.117f, 0.373f, 0.100f)) };
            m4.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_4.obj", 1.0f);


            Mesh m5 = new Mesh() { Material = new LambertMaterial(new Color(0.61f, 0.62f, 0.06f)) };
            m5.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_5.obj", 1.0f);

            Mesh m6 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m6.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_6.obj", 1.0f);

            Mesh m7 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m7.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_7.obj", 1.0f);


            Mesh m8 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m8.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_8.obj", 1.0f);

            Mesh m9 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m9.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_9.obj", 1.0f);

            Mesh m10 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m10.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_10.obj", 1.0f);

            Mesh m11 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m11.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_11.obj", 1.0f);

            Mesh m12 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m12.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_12.obj", 1.0f);

            Mesh m13 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m13.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_13.obj", 1.0f);

            Mesh m14 = new Mesh() { Material = new LambertMaterial(new Color(0.75f, 0.75f, 0.75f)) };
            m14.CreateMeshFromObjectFile("./geometries/cbox/meshes/cbox_14.obj", 1.0f);



            Objects = new IntersectableList();
        

            //Objects.Add(scene);

            Lights = new List<ILight>();
            AreaLight light = new AreaLight(new Color(15, 15, 15), lightMesh);
            Lights.Add(light);
            //Lights.Add(light);
        }
    }
}
