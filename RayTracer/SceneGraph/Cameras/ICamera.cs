using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.Samplers;

namespace RayTracer.SceneGraph.Cameras
{
    public interface ICamera
    {

        Matrix4 TransformationMatrix { get;}
        float AspectRation { get; set; }
        int ScreenWidth { get; set; }
        int ScreenHeight { get; set; }
        Vector3 Up { get; set; }
        Vector3 Eye { get; set; }
        Vector3 LookAt { get; set; }
        float FieldOfViewX { get; set; }
        float FieldOfViewY { get; set; }
        float FieldOfView { get; set; }
        void PreProcess();
        Ray CreateRay(float x, float y);
        Ray CreateRay(float x, float y, List<Sample> appertureSamples);
        float Apperture { get; set; }
        float D { get; set; }
        ICamera Clone();

    }
}
