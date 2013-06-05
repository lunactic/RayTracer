using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;
using RayTracer.Samplers;
using RayTracer.Helper;

namespace RayTracer.SceneGraph.Cameras
{
    public class DepthOfFieldCamera : ICamera
    {
        public Vector3 Up { get; set; }
        public Vector3 Eye { get; set; }
        public Vector3 LookAt { get; set; }
        public Vector3 LookDirection { get; set; }
        public Matrix4 TransformationMatrix { get; private set; }

        public Vector3 u { get; set; }
        public Vector3 v { get; set; }
        public Vector3 w { get; set; }

        public float t { get; set; }
        public float b { get; set; }
        public float r { get; set; }
        public float l { get; set; }

        public float Apperture { get; set; }
        public float D { get; set; }

        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }

        public float FieldOfViewY { get; set; }
        public float FieldOfViewX { get; set; }
        public float FieldOfView { get; set; }
        public float AspectRation { get; set; }

        Random rand = new Random();
        public DepthOfFieldCamera()
        {
            AspectRation = float.NaN;
            FieldOfViewX = float.NaN;
            Apperture = 0.5f;

        }

        public void PreProcess()
        {

            if (float.IsNaN(AspectRation))
            {
                AspectRation = ScreenWidth / ScreenHeight;
            }

            if (!float.IsNaN(FieldOfViewX))
            {
                FieldOfViewX = (float)(FieldOfViewX * 2 * Math.PI / 180f);
                FieldOfViewY = (float)(FieldOfViewY * 2 * Math.PI / 180f);
            }
            else
            {
                FieldOfView = (float)(FieldOfView * 2 * Math.PI / 180f);
            }
            w = Eye - LookAt;
            w.Normalize();
            u = Vector3.Cross(Up, w);
            u.Normalize();
            v = Vector3.Cross(w, u);

            TransformationMatrix = new Matrix4()
            {
                Row0 = new Vector4(u),
                Row1 = new Vector4(v),
                Row2 = new Vector4(w),
                Row3 = new Vector4(Eye) { W = 1 }
            };
            if (!float.IsNaN(FieldOfViewX))
            {
                t = (float)Math.Tan(FieldOfViewX);
                b = -1 * t;
                r = (float)Math.Tan(FieldOfViewY / AspectRation);
                l = -1 * r;
            }
            else
            {
                t = (float)(Math.Tan(FieldOfView / 2f));
                b = -1 * t;
                r = AspectRation * t;
                l = -1 * r;
            }
        }

        public Ray CreateRay(float x, float y, List<Sample> appertureSamples)
        {
            //Calculate Ray in pixel coordinates
            float rU = (l + (r - l) * (x + 0.5f) / ScreenWidth);
            float rV = (b + (t - b) * (y + 0.5f) / ScreenHeight);
            Vector4 cameraRay = new Vector4(rU, rV, -1, 0);

            Vector3 lensRadiusX = Vector3.Cross(LookAt, Up) * (Apperture / 2f);
            Vector3 lensRadiusY = Up * (Apperture / 2f);
            Sample appertureSample = Randomizer.PickRandomSample(appertureSamples);
            Vector3 lensPos = Eye - lensRadiusX * appertureSample.X + lensRadiusY * appertureSample.Y;
            

            #region new approach
            float ft = D / cameraRay.Z;
            Vector3 focusPoint = Eye - cameraRay * ft;

            Vector3 direction = TransformationMatrix.Transform(new Vector4(focusPoint));

            return new Ray(lensPos, direction);

            #endregion

        }

        public ICamera Clone()
        {
            return null;
        }






        public Ray CreateRay(float x, float y)
        {
            throw new NotImplementedException();
        }
    }
}
