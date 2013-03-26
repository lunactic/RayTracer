using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace RayTracer.SceneGraph
{
    public class Tonemapper
    {
        public static void SaveImage(String path, Film film)
        {
            Bitmap bmp = new Bitmap(film.Width, film.Height, PixelFormat.Format32bppArgb);

            for (int i = 0; i < film.Width; i++)
            {
                for (int j = 0; j < film.Height; j++)
                {
                    Structs.Color color = film.GetPixel(i, j);
                    bmp.SetPixel(i,film.Height-1-j,color.GetSystemColor());
                }
            }
            var m = new MemoryStream();
            bmp.Save(m, ImageFormat.Jpeg);

            var img = Image.FromStream(m);

            //TEST
            img.Save(path);


        }
    }
}
