using System.Drawing.Imaging;
using RayTracer.Helper;
using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Materials
{
    public class Texture
    {
        public Bitmap TextureImage { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsBumpMap { get; private set; }


        public Texture(String filename, bool isBumpMap)
        {
            Image img = new Bitmap(filename);
            TextureImage = new Bitmap(img);
            IsBumpMap = isBumpMap;
            Width = TextureImage.Width;
            Height = TextureImage.Height;
        }

        public RayTracer.Structs.Color GetColorFromTexCoordinate(Vector2 texCoord)
        {
            lock (this)
            {
                float u = texCoord.X * (Width - 1);
                float v = (1 - texCoord.Y) * (Height - 1);

                int u0 = (int)Math.Floor(u);
                int v0 = (int)Math.Floor(v);
                int u1 = (int)Math.Ceiling(u);
                int v1 = (int)Math.Ceiling(v);

                u0 = Math.Min(Math.Max(u0, 0), Width - 1);
                u1 = Math.Min(Math.Max(u1, 0), Width - 1);
                v0 = Math.Min(Math.Max(v0, 0), Height - 1);
                v1 = Math.Min(Math.Max(v1, 0), Height - 1);

                float wu = (u - u0) / (u1 - u0);
                float wv = (v - v0) / (v1 - v0);

                int r1 = (int)(TextureImage.GetPixel(u0, v0).R * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v0).R * wu);
                int r2 = (int)(TextureImage.GetPixel(u0, v1).R * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v1).R * wu);

                int g1 = (int)(TextureImage.GetPixel(u0, v0).G * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v0).G * wu);
                int g2 = (int)(TextureImage.GetPixel(u0, v1).G * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v1).G * wu);

                int b1 = (int)(TextureImage.GetPixel(u0, v0).B * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v0).B * wu);
                int b2 = (int)(TextureImage.GetPixel(u0, v1).B * (1 - wu)) + (int)(TextureImage.GetPixel(u1, v1).B * wu);

                int finalR = (int)(r1 * (1 - wv) + r2 * wv);
                int finalG = (int)(g1 * (1 - wv) + g2 * wv);
                int finalB = (int)(b1 * (1 - wv) + b2 * wv);

                return new Structs.Color(finalR / 255f, finalG / 255f, finalB / 255f);
            }
        }
    }
}
