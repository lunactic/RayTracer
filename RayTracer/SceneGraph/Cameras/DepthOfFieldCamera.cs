using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Cameras
{
    public class DepthOfFieldCamera : ICamera
    {
        public Vector4 Up { get; set; }
        public Vector4 Eye { get; set; }
        public Vector4 LookAt { get; set; }
        public Vector4 LookDirection { get; private set; }
        public float FieldOfView { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public float AspectRation { get; private set; }


        private Vector4 u;
        private Vector4 v;
        private Vector4 w;
        private float t;
        private float b;
        private float r;
        private float l;
        private Matrix4 transformationMatrix;

        public DepthOfFieldCamera()
        {
            
        }
        public void PreProcess()
        {

            AspectRation = ScreenWidth / ScreenHeight;
            LookDirection = LookAt - Eye;
            LookDirection.Normalize();
            FieldOfView = (float)(FieldOfView * 2 * Math.PI / 180f);
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
            t = (float)Math.Tan(FieldOfView / 2);
            b = -1 * t;
            r = AspectRation * t;
            l = -1 * r;
        }

        public Ray CreateRay(float x, float y)
        {
            //Calculate Ray in pixel coordinates
            float rU = (l + (r - l) * (x + 0.5f) / ScreenWidth);
            float rV = (b + (t - b) * (y + 0.5f) / ScreenHeight);

            Vector4 cameraRay = new Vector4(rU, rV, -1, 0);
            Vector4 direction = Vector4.Transform(cameraRay, transformationMatrix);
            direction = direction - Eye;

            //Calculate the point in focus
            Vector4 pointAimed = Eye + direction;
            direction.Normalize();
            

            Random rand = new Random();
            //Calculateone jittered dispersion
            float du = (float) rand.NextDouble();
            float dv = (float) rand.NextDouble();
            //Create new Camera position;
            
            Vector4 newStart = Eye - (u*0.5f - v*0.5f + Vector4.Multiply(u,du) + Vector4.Multiply(v,dv));
            Vector4 newDirection = pointAimed - newStart;

            newDirection.Normalize();

            //newDirection = Vector4.Transform(newDirection, transformationMatrix);

            return new Ray(newStart,newDirection);

        }

        public ICamera Clone()
        {
            DepthOfFieldCamera clone = new DepthOfFieldCamera()
                {
                    Up = Up,
                    Eye = Eye,
                    LookAt = LookAt,
                    LookDirection = LookDirection,
                    FieldOfView = FieldOfView,
                    ScreenHeight = ScreenHeight,
                    ScreenWidth = ScreenWidth
                };
            clone.PreProcess();
            clone.FieldOfView = FieldOfView;
            return clone;
        }
    }
}
