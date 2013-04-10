using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Samplers
{
    public class Sample
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Sample(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
