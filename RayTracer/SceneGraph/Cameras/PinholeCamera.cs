using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Structs;

namespace RayTracer.SceneGraph.Cameras
{
    public class PinholeCamera : ICamera
    {
        #region Properties
        public Vector4 Up { get; set; }
        public Vector4 Eye { get; set; }
        public Vector4 LookAt { get; set; }
        public Vector4 LookDirection { get; private set; }
        public float FieldOfViewX { get; set; }
        public float FieldOfViewY { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public float AspectRation { get; set; }

        public float Apperture { get; set; }
        public float D { get; set; }

        #endregion

        #region fields

        private Vector4 u;
        private Vector4 v;
        private Vector4 w;
        private float t;
        private float b;
        private float r;
        private float l;
        private Matrix4 transformationMatrix;
        #endregion


        public PinholeCamera()
        {
            AspectRation = float.NaN;
        }

        public void PreProcess()
        {
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

        /// <summary>
        /// Creates a Ray at a given pixel position
        /// </summary>
        /// <param name="x">x position of the pixel</param>
        /// <param name="y">y position of the pixel</param>
        /// <returns>A Ray from the Camera center trough the pixel</returns>
        public Ray CreateRay(float x, float y)
        {

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
            PinholeCamera clone = new PinholeCamera()
            {
                Up = Up,
                Eye = Eye,
                LookAt = LookAt,
                FieldOfViewX = (float)(FieldOfViewX / 2 / Math.PI * 180f),
                FieldOfViewY = (float)(FieldOfViewY / 2 / Math.PI * 180f),
                ScreenHeight = ScreenHeight,
                ScreenWidth = ScreenWidth,

            };
            clone.PreProcess();
            return clone;
        }
    }
}
