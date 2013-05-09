using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public class Sample
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Sample(float x, float y)
        {
            X = x;
            Y = y;
        }

    }
}
