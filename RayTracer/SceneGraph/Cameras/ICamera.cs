using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Cameras
{
    public interface ICamera
    {
        float AspectRation { get; set; }
        int ScreenWidth { get; set; }
        int ScreenHeight { get; set; }
        Vector4 Up { get; set; }
        Vector4 Eye { get; set; }
        Vector4 LookAt { get; set; }
        float FieldOfViewX { get; set; }
        float FieldOfViewY { get; set; }
        void PreProcess();
        Ray CreateRay(float x, float y);
        float Apperture { get; set; }
        float D { get; set; }
        ICamera Clone();
    }
}
