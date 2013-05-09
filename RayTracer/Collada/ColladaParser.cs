using System;
using System.Collections.Generic;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
namespace RayTracer.Collada
{
    internal enum MaterialType
    {
        Lambert, Blinn, Mirror, Refractive
    }

    public class ColladaParser
    {
        public Dictionary<String, ICamera> Cameras { get; private set; }
        public Dictionary<String, Mesh> Meshes { get; private set; }
        public Dictionary<String, ILight> Lights { get; private set; }
        public Dictionary<String, Material> Materials { get; private set; }

        public void ParseColladaFile(String filename)
        {
            Cameras = new Dictionary<string, ICamera>();
            Meshes = new Dictionary<string, Mesh>();
            Lights = new Dictionary<string, ILight>();
            Materials = new Dictionary<string, Material>();
            XElement xdocument = XElement.Load(filename);
            var allnode = xdocument.Elements();

            var cameraNode = from item in allnode.DescendantsAndSelf("library_cameras") select item;
            var cameraInfos = from item in allnode.DescendantsAndSelf("library_visual_scenes").DescendantsAndSelf("node") where item.Attribute("id").Value == "Camera" select item;
            var materialNodes = from item in allnode.DescendantsAndSelf("library_effects") select item;
            var geometryNodes = from item in allnode.DescendantsAndSelf("library_geometries") select item;
            ParseCamera(cameraNode, cameraInfos);
            ParseMaterials(materialNodes);
            ParseGeometries(geometryNodes);
            Console.Write("BLUBB");
        }

        private void ParseGeometries(IEnumerable<XElement> geometryNodes)
        {
            foreach (var geometry in geometryNodes.Elements("geometry"))
            {
                String geometryName = geometry.Attribute("name").Value;
                Dictionary<String, float[]> floatArrays = new Dictionary<string, float[]>();
                Dictionary<String, String> positionToVertex = new Dictionary<string, string>();
                //Parse the Float Arrays
                foreach (var source in geometry.Element("mesh").Elements("source"))
                {
                    String key = source.Attribute("id").Value;
                    float[] values = ParseFloatArray(source.Element("float_array").Value);
                    floatArrays.Add(key, values);
                }
                //Get the vertices
                foreach (var source in geometry.Element("mesh").Elements("vertices"))
                {
                    String vertexID = source.Attribute("id").Value;
                    foreach (var element in source.Elements("input"))
                    {
                        String positionId = element.Attribute("source").Value.Remove(0, 1);
                        positionToVertex.Add(vertexID, positionId);
                    }
                }
                //Parse the Triangle

                #region TriangleMesh

                if (geometry.Element("mesh").Elements("triangles").Count() > 0)
                {
                    int i = 0;
                    foreach (var source in geometry.Element("mesh").Elements("triangles"))
                    {
                        int triangleCount = Convert.ToInt32(source.Attribute("count").Value);
                        String materialName = source.Attribute("material").Value;
                        //Parse Everything
                        IEnumerable<string> vertexArrayNames = from item in source.Elements("input")
                                                               where item.Attribute("semantic").Value == "VERTEX"
                                                               select item.Attribute("source").Value.Remove(0, 1);
                        IEnumerable<string> vertexArrayOffsets = from item in source.Elements("input")
                                                                 where item.Attribute("semantic").Value == "VERTEX"
                                                                 select item.Attribute("offset").Value;
                        IEnumerable<string> normalArrayNames = from item in source.Elements("input")
                                                               where item.Attribute("semantic").Value == "NORMAL"
                                                               select item.Attribute("source").Value.Remove(0, 1);
                        IEnumerable<string> normalArrayOffsets = from item in source.Elements("input")
                                                                 where item.Attribute("semantic").Value == "NORMAL"
                                                                 select item.Attribute("offset").Value;
                        String normalArrayName = normalArrayNames.First();
                        String vertexArrayName = vertexArrayNames.First();
                        int vertexOffset;
                        int.TryParse(vertexArrayOffsets.First(), out vertexOffset);
                        int normalOffset;
                        int.TryParse(normalArrayOffsets.First(), out normalOffset);



                        float[] vertices = floatArrays[positionToVertex[vertexArrayName]];
                        float[] normals = floatArrays[normalArrayName];

                        String indices = source.Element("p").Value;

                        int[] vertexIndices = ParseTriangleIndices(indices, vertexOffset, 2, triangleCount);
                        int[] normalIndices = ParseTriangleIndices(indices, normalOffset, 2, triangleCount);

                        //Create the Mesh
                        Material mat = Materials[materialName];
                        Mesh m = new Mesh(vertices, vertexIndices, normals, normalIndices, mat);
                        Meshes.Add(geometryName+i, m);
                        i++;
                    }
                }

                #endregion


            }
        }

        private int[] ParseTriangleIndices(string indices, int offset, int numberOfInputs, int triangleCount)
        {
            String[] allValues = indices.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] returnValues = new int[triangleCount * 3];
            int j = offset;
            for (int i = 0; i < triangleCount * 3; i++)
            {
                returnValues[i] = Convert.ToInt32(allValues[j]);
                j += numberOfInputs;
            }
            return returnValues;
        }

        private float[] ParseFloatArray(string values)
        {
            String[] sValues = values.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            float[] fValues = new float[sValues.Length];
            int i = 0;
            foreach (var sValue in sValues)
            {
                float value;
                float.TryParse(sValue, out value);
                fValues[i] = value;
                i++;
            }
            return fValues;
        }

        private void ParseCamera(IEnumerable<XElement> cameraNode, IEnumerable<XElement> cameraInfos)
        {
            foreach (var node in cameraNode.Descendants("camera"))
            {
                String cameraName = node.Attribute("name").Value;
                ICamera cam = new PinholeCamera();
                float xfov, yfov;
                float.TryParse(node.Element("optics").Element("technique_common").Element("perspective").Element("xfov").Value, out xfov);
                float.TryParse(node.Element("optics").Element("technique_common").Element("perspective").Element("yfov").Value, out yfov);
                //float aspectRatio = (float)(Math.Tan(0.5f * xfov) / Math.Tan(0.5f * yfov));
                cam.AspectRation = xfov / yfov;

                cam.FieldOfViewX = xfov;
                cam.FieldOfViewY = yfov;
                Cameras.Add(cameraName, cam);
            }

            var cameraNames = from item in cameraInfos.Elements("instance_camera") select item.Attribute("url").Value;



            foreach (String camName in cameraNames)
            {
                String name = camName.Remove(0, 1);
                ICamera cam = Cameras[name];
                String[] lookAt = (from item in cameraInfos.Elements() where item.Name == "lookat" select item.Value).First().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 3; i++)
                {
                    String values = lookAt[i].TrimStart();
                    String[] lookAtValues = values.Split(new char[] { ' ' });
                    float x, y, z;
                    float.TryParse(lookAtValues[0], out x);
                    float.TryParse(lookAtValues[1], out y);
                    float.TryParse(lookAtValues[2], out z);

                    Vector4 lookAtVector = new Vector4(x, y, z, 1);
                    switch (i)
                    {
                        case 0:
                            cam.Eye = lookAtVector;
                            break;
                        case 1:
                            cam.LookAt = lookAtVector;
                            break;
                        case 2:
                            cam.Up = lookAtVector;
                            break;
                    }
                }
                cam.ScreenWidth = 512;
                cam.ScreenHeight = (int)(512 / cam.AspectRation);
                cam.PreProcess();
            }

        }

        private void ParseMaterials(IEnumerable<XElement> materialNodes)
        {
            int i = 0;
            foreach (var material in materialNodes.Elements("effect"))
            {
                String materialName = material.Attribute("name").Value.Replace("-effect", "");
                Console.WriteLine(i++);

                //Determine the MaterialType
                foreach (var subNode in material.Element("profile_COMMON").Element("technique").Elements())
                {
                    if (subNode.Name == "lambert")
                    {
                        Color diffuse = ParseColor(subNode.Element("diffuse").Element("color").Value);
                        Material mat = new LambertMaterial(diffuse);
                        //Create lambert material
                        Materials.Add(materialName, mat);

                    }
                    if (subNode.Name == "blinn")
                    {

                        //Create BlinnPhong Material
                    }
                    if (subNode.Name == "phong")
                    {
                        //Create Refractive Material
                    }

                }
            }
        }

        private Color ParseColor(String value)
        {
            String[] values = value.Split(new char[] { ' ' });

            float r, g, b;

            float.TryParse(values[0], out r);
            float.TryParse(values[1], out g);
            float.TryParse(values[2], out b);
            return new Color(r, g, b);

        }
    }
}