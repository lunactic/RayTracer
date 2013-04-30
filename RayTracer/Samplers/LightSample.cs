using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public class LightSample
    {

        public Color LightColor { get; set; }
        public Vector3 Wi { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Position { get; set; }

        public float Pdf { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Distance { get; set; }
        public float Area { get; set; }


        public LightSample(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
