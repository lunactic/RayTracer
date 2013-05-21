using RayTracer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneGraph.Materials
{
    public static class SkyBox
    {
        private static Texture Left;
        private static Texture Right;
        private static Texture Top;
        private static Texture Bottom;
        private static Texture Front;
        private static Texture Back;

        private static bool hasLoadedSkybox = false;

   
        public static void LoadSkybox(Texture left, Texture right, Texture top, Texture bottom, Texture front, Texture back)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Front = front;
            Back = back;
            hasLoadedSkybox = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Color GetSkyBoxColor(Ray ray)
        {
            Color retColor = new Color(0, 0, 0);
            Vector3 direction = ray.Direction;
            if (Math.Abs(direction.X) >= Math.Abs(direction.Y) && Math.Abs(direction.X) >= Math.Abs(direction.Z))
            {
                if (direction.X > 0f)
                {
                    retColor = Left.GetColorFromTexCoordinate(new Vector2(1.0f - (direction.Z / direction.X + 1.0f) * 0.5f, 1.0f - (direction.Y / direction.X + 1.0f) * 0.5f));
                }
                else if (direction.X < 0f)
                {
                    retColor = Right.GetColorFromTexCoordinate(new Vector2(1.0f - (direction.Z / direction.X + 1.0f) * 0.5f, (direction.Y / direction.X + 1.0f) * 0.5f));
                }
            }
            else if ((Math.Abs(direction.Y) >= Math.Abs(direction.X)) && (Math.Abs(direction.Y) >= Math.Abs(direction.Z)))
            {
                if (direction.Y > 0f)
                {
                    retColor = Top.GetColorFromTexCoordinate(new Vector2((direction.X / direction.Y + 1.0f) * 0.5f, (direction.Z / direction.Y + 1.0f) * 0.5f));
                }
                else if (direction.Y < 0f)
                {
                    retColor = Bottom.GetColorFromTexCoordinate(new Vector2(1.0f - (direction.X / direction.Y + 1.0f) * 0.5f, (direction.Z / direction.Y + 1.0f) * 0.5f));
                }
            }
            else if ((Math.Abs(direction.Z) >= Math.Abs(direction.X)) && (Math.Abs(direction.Z) >= Math.Abs(direction.Y)))
            {
                if (direction.Z > 0f)
                {
                    retColor = Front.GetColorFromTexCoordinate(new Vector2((direction.X / direction.Z + 1.0f) * 0.5f, 1.0f - (direction.Y / direction.Z + 1.0f) * 0.5f));
                }
                else if (direction.Z < 0f)
                {
                    retColor = Back.GetColorFromTexCoordinate(new Vector2((direction.X / direction.Z + 1.0f) * 0.5f, (direction.Y / direction.Z + 1) * 0.5f));
                }
            }


            return retColor;
        }
        

        public static bool IsSkyBoxLoaded()
        {
            return hasLoadedSkybox;
        }
    }
}
