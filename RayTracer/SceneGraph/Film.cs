using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph
{
    public class Film
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Color[,] Image;

        public Film(int width, int height)
        {
            Width = width;
            Height = height;
            Image = new Color[width,height];
        }

        public void SetPixel(int x, int y, Color color)
        {
            Image[x,y] = color;
        }

        public Color GetPixel(int x, int y)
        {
            return Image[x,y];
        }
    }
}
