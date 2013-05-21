using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace RayTracer.Helper
{
    public class ObjReader
    {
        
        public static void Read(String filename, float scale, List<float[]> vertices, List<float[]> texCoords,
                                        List<float[]> normals, List<int[,]> faces)
        {
            TextReader textReader = new StreamReader(filename);

           
            float xMin, xMax, yMin, yMax, zMin, zMax;
            xMin = float.MaxValue;
            xMax = float.Epsilon;
            yMin = float.MaxValue;
            yMax = float.Epsilon;
            zMin = float.MaxValue;
            zMax = float.Epsilon;

            String line = null;

            while ((line = textReader.ReadLine()) != null)
            {
                //Read Line
                string[] s = Regex.Split(line, "\\s+");
                if (s[0].CompareTo("v") == 0)
                {

                    //Position
                    float[] v = new float[3];
                    v[0] = float.Parse(s[1]);
                    v[1] = float.Parse(s[2]);
                    v[2] = float.Parse(s[3]);
                    vertices.Add(v);

                    // Update extent
                    if (v[0] < xMin) xMin = v[0];
                    if (v[0] > xMax) xMax = v[0];
                    if (v[1] < yMin) yMin = v[1];
                    if (v[1] > yMax) yMax = v[1];
                    if (v[2] < zMin) zMin = v[2];
                    if (v[2] > zMax) zMax = v[2];
                }
                else if (s[0].CompareTo("vn") == 0)
                {
                    //Normal
                    float[] n = new float[3];
                    n[0] = float.Parse(s[1]);
                    n[1] = float.Parse(s[2]);
                    n[2] = float.Parse(s[3]);
                    normals.Add(n);
                }
                else if (s[0].CompareTo("vt") == 0)
                {
                    //Texture
                    float[] t = new float[2];
                    t[0] = float.Parse(s[1]);
                    t[1] = float.Parse(s[2]);
                }
                else if (s[0].CompareTo("f") == 0)
                {
                    //Indices
                    int[,] indices = new int[3,3];
                    //For all vertices
                    int i=1;
                    while(i<s.Length)
                    {
                        //Get indices for vertex position, tex. coords., and normals
                        String[] ss = s[i].Split(new string[1]{"/"},StringSplitOptions.None);
                        int k=0;
                        while(k<ss.Length)
                        {
                            if(ss[k].Length>0)
                                indices[i-1,k] = int.Parse(ss[k]);
                            else
                            {
                                indices[i-1,k] = -1;
                            }
                            k++;
                        }
                        i++;
                    }
                    faces.Add(indices);
                }
                else if(s[0].Length > 0 && s[0].ElementAt(0)!='#')
                {
                    Console.WriteLine("Unknown token: " + line + "\n");
                }
            }

            //Normalization
            
            float xTrans = -(xMax+xMin)/2;
		    float yTrans = -(yMax+yMin)/2;
		    float zTrans = -(zMax+zMin)/2;
		    float xScale = 2/(xMax-xMin);
		    float yScale = 2/(yMax-yMin);
		    float zScale = 2/(zMax-zMin);
		    float r = yScale;
		    if(xScale < yScale) r = xScale;
		    if(zScale < r) r = zScale;
		    scale = r*scale;

            for (int i = 0; i < vertices.Count; i++)
            {
                float[] v = vertices[i];
                v[0] = scale * (v[0] + xTrans);
                v[1] = scale * (v[1] + yTrans);
                v[2] = scale * (v[2] + zTrans);
                vertices[i] = v;
            }
            
        }
    }
       
}
