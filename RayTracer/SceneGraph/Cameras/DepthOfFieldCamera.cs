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
        public Vector4 Up { get; set; }
        public Vector4 Eye { get; set; }
        public Vector4 LookAt { get; set; }
        public Vector4 LookDirection { get; set; }

        public Vector4 u { get; set; }
        public Vector4 v { get; set; }
        public Vector4 w { get; set; }

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

        public float AspectRation { get; set; }

        private List<Sample> pixelSamples;
        private Matrix4 transformationMatrix;
        public DepthOfFieldCamera()
        {
             pixelSamples = ((ISampler)Activator.CreateInstance(Constants.Sampler)).CreateSamples(); //x_samples
            
        }

        public void PreProcess()
        {
            /*
            LookDirection = LookAt - Eye;
            LookDirection.Normalize(); //cam_f
            v = new Vector4(Vector3.Cross(LookDirection, Up));
            Vector3 vv = v;
            vv.Normalize();
            v = new Vector4(vv);
            w = new Vector4(Vector3.Cross(v, LookDirection));
            Vector3 ww = w;
            ww.Normalize();
            w = new Vector4(ww); //cam_u
            */
            if (float.IsNaN(AspectRation))
            {
                AspectRation = ScreenWidth / ScreenHeight;
            }

            LookDirection = LookAt - Eye;
            LookDirection.Normalize();
            FieldOfViewX = (float)(FieldOfViewX * 2 * Math.PI / 180f);
            FieldOfViewY = (float)(FieldOfViewY * 2 * Math.PI / 180f);
            w = Eye - LookAt;
            w.Normalize();
            u = new Vector4(Vector3.Cross(Up, w));
            u.Normalize();
            v = new Vector4(Vector3.Cross(w, u));

            transformationMatrix = new Matrix4()
            {
                Row0 = u,
                Row1 = v,
                Row2 = w,
                Row3 = Eye
            };

            t = (float)Math.Tan(FieldOfViewX);
            b = -1 * t;
            r = (float)Math.Tan(FieldOfViewY / AspectRation);
            l = -1 * r;
        }

        public Ray CreateRay(float x, float y)
        {
            //#define VectorMA(a, s, b, c)	(c[0]=a[0]+(s)*b[0],c[1]=a[1]+(s)*b[1],c[2]=a[2]+(s)*b[2])
            //VectorMA(start, cam.aperature*xl_samples[s], cam_r, start);
           
            //Calculate Ray in pixel coordinates
            float rU = (l + (r - l) * (x + 0.5f) / ScreenWidth);
            float rV = (b + (t - b) * (y + 0.5f) / ScreenHeight);

            Vector4 cameraRay = new Vector4(rU, rV, -1, 0);
            Vector4 direction = Vector4.Transform(cameraRay, transformationMatrix);
            direction = direction - Eye;

            return new Ray(Eye, direction);

        }

        public ICamera Clone()
        {
            return null;
        }


 
        
    }
}
