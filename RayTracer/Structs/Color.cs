using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    public class Color
    {
      /// <summary>
        /// The red component
        /// </summary>
        public float R;
        /// <summary>
        /// The green component
        /// </summary>
        public float G;
        /// <summary>
        /// The blue component
        /// </summary>
        public float B;


        public float Power
        {
            get
            {
                Vector3 v = new Vector3(R, G, B);
                return v.Length * v.Length;
            }
        }

        public System.Drawing.Color GetSystemColor()
        {
            int r = (int)Math.Floor((double)((R) >= 1.0 ? 255 : (int)((R) * 256.0)));
            int g = (int)Math.Floor((double)((G) >= 1.0 ? 255 : (int)((G) * 256.0)));
            int b = (int)Math.Floor((double)((B) >= 1.0 ? 255 : (int)((B) * 256.0)));
            r = Math.Min(r, 255);
            g = Math.Min(g, 255);
            b = Math.Min(b, 255);
            r = Math.Max(0, r);
            g = Math.Max(0, g);
            b = Math.Max(0, b);
            return System.Drawing.Color.FromArgb(1, r, g, b);
        }

        /// <summary>
        /// Constructs a new Color structure from the specified components.
        /// </summary>
        /// <param name="r">The red component of the new Color structure.</param>
        /// <param name="g">The green component of the new Color structure.</param>
        /// <param name="b">The blue component of the new Color structure.</param>
        /// <param name="a">The alpha component of the new Color structure.</param>
        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void Append(Color c)
        {
            R += c.R;
            G += c.G;
            B += c.B;

        }
       public Color Div(float d)
       {
           return new Color(R/d,G/d,B/d);
       }

        public void VoidDiv(float d)
        {
            R = R/d;
            G = G/d;
            B = B/d;
        }

        public Color Mult(float d)
        {
            return new Color(R*d,G*d,B*d);
        }

        public Color Mult(Color c)
        {
            return new Color(R*c.R,G*c.G,B*c.B);
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                Color other = (Color)obj;
                return other.R == R && other.G == G && other.B == B;
            }
            return false;
        }

        public void Clamp(float min, float max)
        {
            R = Math.Max(R, min);
            G = Math.Max(G, min);
            B = Math.Max(B, min);


            R = Math.Min(R, max);
            G = Math.Min(G, max);
            B = Math.Min(B, max);
        }
    }
}
