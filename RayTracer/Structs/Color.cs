using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Structs
{
    public struct Color
    {
        /// <summary>
        /// Red Color
        /// </summary>
        public static readonly Color Red = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        /// <summary>
        /// Green Color
        /// </summary>
        public static readonly Color Green = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        /// <summary>
        /// Blue Color
        /// </summary>
        public static readonly Color Blue = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        /// <summary>
        /// White Color
        /// </summary>
        public static readonly Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        /// <summary>
        /// Black Color
        /// </summary>
        public static readonly Color Black = new Color(0.0f, 0.0f, 0.0f, 1.0f);

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
        /// <summary>
        /// The alpha component
        /// </summary>
        public float A;
        
        public System.Drawing.Color GetSystemColor()
        {
            int r = (int) Math.Floor((double) ((R) >= 1.0 ? 255 : (int)((R)*256.0)));
            int g = (int)Math.Floor((double)((G) >= 1.0 ? 255 : (int)((G) * 256.0)));
            int b = (int)Math.Floor((double)((B) >= 1.0 ? 255 : (int)((B) * 256.0)));
            int a = (int)Math.Floor((double)((A) >= 1.0 ? 255 : (int)((A) * 256.0)));
            r = Math.Min(r, 255);
            g = Math.Min(g, 255);
            b = Math.Min(b, 255);
            r = Math.Max(0, r);
            g = Math.Max(0, g);
            b = Math.Max(0, b);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Constructs a new Color structure from the specified components.
        /// </summary>
        /// <param name="r">The red component of the new Color structure.</param>
        /// <param name="g">The green component of the new Color structure.</param>
        /// <param name="b">The blue component of the new Color structure.</param>
        /// <param name="a">The alpha component of the new Color structure.</param>
        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Ch.Fhnw.ViewerComponentLibrary.Structs.Color"/> to <see cref="System.Single[]"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float[](Color color)
        {
            return new float[] { color.R, color.G, color.B, color.A };
        }

        /// <summary>
        /// Multiplies a color value with a constant (without changing the ambient value!)
        /// </summary>
        /// <param name="c">The color</param>
        /// <param name="scale">The scale factor</param>
        /// <returns>The scaled color value</returns>
        public static Color operator *(Color c, float scale)
        {
            return new Color(c.R * scale, c.G * scale, c.B * scale, c.A);
  
        }

        /// <summary>
        /// Adds two colors togheter
        /// </summary>
        /// <param name="l">The first color</param>
        /// <param name="r">The second color</param>
        /// <returns>The additive color, with the ambient value of l</returns>
        public static Color operator +(Color l, Color r)
        {
            return new Color(l.R + r.R, l.G + r.G, l.B + r.B, l.A);
        }
    
        /// <summary>
        /// Multiplies two colors
        /// </summary>
        /// <param name="l">The first color</param>
        /// <param name="r">The second color</param>
        /// <returns>The multiplied color values</returns>
        public static Color operator *(Color l, Color r)
        {
            return new Color(l.R * r.R, l.G * r.G, l.B * r.B, l.A);
        }
        
        public static Color operator /(Color l, int num)
        {
            return new Color(l.R/num,l.G/num,l.B/num,l.A);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return R.GetHashCode() + G.GetHashCode() + B.GetHashCode() + A.GetHashCode();
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
                return other.R == R && other.G == G && other.B == B && other.A == A;
            }
            return false;
        }
        
        public void Clamp(float min, float max)
        {
            A = Math.Max(A, min);
            R = Math.Max(R, min);
            G = Math.Max(G, min);
            B = Math.Max(B, min);


            A = Math.Min(A, max);
            R = Math.Min(R, max);
            G = Math.Min(G, max);
            B = Math.Min(B, max);
        }
    }
}
