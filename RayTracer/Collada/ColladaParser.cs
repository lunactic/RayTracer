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

        private Dictionary<string, string> symbolToId = new Dictionary<string, string>();
        private Dictionary<String, String> materialsToEffects = new Dictionary<string, string>();
        private Dictionary<string, string> nodeIdToGeometryName = new Dictionary<string, string>();
        private Dictionary<string, string> materialSymbolToId = new Dictionary<string, string>();

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
            var materialInfos = from item in allnode.DescendantsAndSelf("library_materials") select item;
            var geometryNodes = from item in allnode.DescendantsAndSelf("library_geometries") select item;
            var geometryInfos = from item in allnode.DescendantsAndSelf("library_nodes") select item;

            if (cameraNode.Count() > 0 && cameraInfos.Count() > 0)
                ParseCamera(cameraNode, cameraInfos);
            ParseMaterials(materialNodes, materialInfos);
            ParseGeometries(geometryNodes, geometryInfos);
        }

        private void ParseGeometries(IEnumerable<XElement> geometryNodes, IEnumerable<XElement> geometryInfos)
        {
            bool hasLibraryNodes = true;
            #region parse the necessary infos
            if (geometryInfos.Count() == 0)
            {
                hasLibraryNodes = false;
            }
            foreach (var node in geometryInfos.Elements("node"))
            {
                String id = node.Attribute("id").Value;
                foreach (var instanceGeometry in node.Elements("instance_geometry"))
                {
                    String url = instanceGeometry.Attribute("url").Value.Replace("#", "");
                    nodeIdToGeometryName.Add(url, id);
                    XElement bind_material = instanceGeometry.Element("bind_material");
                    foreach (var instance_material in bind_material.Element("technique_common").Elements("instance_material"))
                    {
                        String symbol = instance_material.Attribute("symbol").Value;
                        String materialId = instance_material.Attribute("target").Value.Replace("#", "");
                        materialSymbolToId.Add(symbol, materialId);
                    }
                }
            }
            #endregion

            foreach (var geometry in geometryNodes.Elements("geometry"))
            {
                String geometryId = geometry.Attribute("id").Value;
                Dictionary<String, float[]> floatArrays = new Dictionary<string, float[]>();
                Dictionary<String, String> positionToVertex = new Dictionary<string, string>();
                Dictionary<String, String> normalToVertex = new Dictionary<string, string>();
                
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
                        if (element.Attribute("semantic").Value.Equals("POSITION"))
                        {
                            String positionId = element.Attribute("source").Value.Replace("#","");
                            positionToVertex.Add(vertexID, positionId);
                        }
                        if (element.Attribute("semantic").Value.Equals("NORMAL"))
                        {
                            String normalId = element.Attribute("source").Value.Replace("#", "");
                            normalToVertex.Add(vertexID, normalId);
                        }
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
                        String materialName = source.Attribute("material").Value.Replace("-material", "");
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

                        IEnumerable<string> texCordArrayNames = from item in source.Elements("input")
                                                                where item.Attribute("semantic").Value == "TEXCOORD"
                                                                select item.Attribute("source").Value.Remove(0, 1);
                        IEnumerable<string> texCordArrayOffsets = from item in source.Elements("input")
                                                                  where item.Attribute("semantic").Value == "TEXCOORD"
                                                                  select item.Attribute("offset").Value;

                        String normalArrayName = normalArrayNames.FirstOrDefault();
                        String vertexArrayName = vertexArrayNames.FirstOrDefault();
                        String texCordArrayName = texCordArrayNames.FirstOrDefault();
                        int normalOffset = -1;
                        int vertexOffset = -1;
                        int texCoordOffset = -1;

                        float[] normals = null;
                        float[] vertices = null;
                        float[] texCoords = null;

                        int[] vertexIndices = null;
                        int[] normalIndices = null;
                        int[] texCoordIndices = null;
                        String indices = source.Element("p").Value;

                        int count = 0;
                        if (!string.IsNullOrEmpty(normalArrayName) || hasLibraryNodes)
                        {
                            if (hasLibraryNodes)
                            {
                                normalArrayName = vertexArrayName;
                                int.TryParse(vertexArrayOffsets.First(), out normalOffset);
                                normals = floatArrays[normalToVertex[normalArrayName]];
                                //count++;
                            }
                            else
                            {
                                int.TryParse(normalArrayOffsets.First(), out normalOffset);
                                normals = floatArrays[normalArrayName];
                                count++;
                            }
                      
                        }
                        if (!string.IsNullOrEmpty(vertexArrayName))
                        {
                            int.TryParse(vertexArrayOffsets.First(), out vertexOffset);
                            vertices = floatArrays[positionToVertex[vertexArrayName]];
                            count++;
                        }
                        if (!string.IsNullOrEmpty(texCordArrayName))
                        {
                            int.TryParse(texCordArrayOffsets.First(), out texCoordOffset);
                            texCoords = floatArrays[texCordArrayName];
                            count++;
                        }
                        if(!string.IsNullOrEmpty(normalArrayName))
                            normalIndices = ParseTriangleIndices(indices, normalOffset, count, triangleCount);
                        if (!string.IsNullOrEmpty(vertexArrayName))
                            vertexIndices = ParseTriangleIndices(indices, vertexOffset, count, triangleCount);
                        if (!string.IsNullOrEmpty(texCordArrayName))
                            texCoordIndices = ParseTriangleIndices(indices, texCoordOffset, count, triangleCount);
                        //Create the Mesh
                        Material mat;
                        if (hasLibraryNodes)
                            mat = Materials[materialsToEffects[materialSymbolToId[materialName]]];
                        else
                            mat = Materials[materialName];
                        Mesh m = new Mesh(vertices, vertexIndices, normals, normalIndices, texCoords, texCoordIndices, mat);
                        Meshes.Add(geometryId + i, m);
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

        private void ParseMaterials(IEnumerable<XElement> materialNodes, IEnumerable<XElement> materialInfos)
        {

            #region build Material Dictionary
            foreach (var material in materialInfos.Elements("material"))
            {
                String id = material.Attribute("id").Value;
                String reference = material.Element("instance_effect").Attribute("url").Value.Replace("#", "");
                materialsToEffects.Add(id, reference);
            }
            #endregion

            foreach (var material in materialNodes.Elements("effect"))
            {
                String materialName = material.Attribute("id").Value.Replace("-effect", "");
  
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
                        Color diffuse = ParseColor(subNode.Element("diffuse").Element("color").Value);
                        Color specular = ParseColor(subNode.Element("specular").Element("color").Value);
                        float shininess = (float)Convert.ToDouble(subNode.Element("shininess").Element("float").Value);
                        Material mat = new BlinnPhongMaterial(diffuse, specular, shininess);
                        Materials.Add(materialName, mat);
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