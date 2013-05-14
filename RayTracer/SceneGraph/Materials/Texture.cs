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

        public Texture(String filename)
        {
            Image img = new Bitmap(filename);
            TextureImage = new Bitmap(img);

            Width = TextureImage.Width;
            Height = TextureImage.Height;
        }

        public RayTracer.Structs.Color GetColorFromTexCoordinate(Vector2 texCoord)
        {
            int i = (int)((texCoord.X * Width) % Width);
            int j = (int)((texCoord.Y * Height) % Height);
            i = Math.Min(Math.Max(i, 0), Width - 1);
            j = Math.Min(Math.Max(j, 0), Height - 1);

            RayTracer.Structs.Color color = new Structs.Color(0, 0, 0);

            color = new RayTracer.Structs.Color(TextureImage.GetPixel(i, j));
            return color;
        }
    }
}
