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
using RayTracer.SceneGraph.Accelerate;
namespace RayTracer.Collada
{
    internal enum MaterialType
    {
        Lambert, Blinn, Mirror, Refractive
    }

    public class ColladaParser
    {
        public Dictionary<String, ICamera> Cameras { get; private set; }
        public Dictionary<String, IIntersectable> Meshes { get; private set; }
        public Dictionary<String, ILight> Lights { get; private set; }
        public Dictionary<String, Material> Materials { get; private set; }

        private Dictionary<string, string> symbolToId = new Dictionary<string, string>();
        private Dictionary<String, String> materialsToEffects = new Dictionary<string, string>();
        private Dictionary<string, string> nodeIdToGeometryName = new Dictionary<string, string>();
        private Dictionary<string, string> materialSymbolToId = new Dictionary<string, string>();

        private float scale;

        public void ParseColladaFile(String filename)
        {
            Cameras = new Dictionary<string, ICamera>();
            Meshes = new Dictionary<string, IIntersectable>();
            Lights = new Dictionary<string, ILight>();
            Materials = new Dictionary<string, Material>();
            XElement xdocument = XElement.Load(filename,LoadOptions.SetBaseUri);
            var allnode = xdocument.Elements();

            var cameraNode = from item in allnode.DescendantsAndSelf("library_cameras") select item;
            var visualSceneNode = from item in allnode.DescendantsAndSelf("library_visual_scenes").Elements("visual_scene") select item;
            var materialNodes = from item in allnode.DescendantsAndSelf("library_effects") select item;
            var materialInfos = from item in allnode.DescendantsAndSelf("library_materials") select item;
            var geometryNodes = from item in allnode.DescendantsAndSelf("library_geometries") select item;
            var lightNodes = from item in allnode.DescendantsAndSelf("library_lights") select item;
            var imageNodes = from item in allnode.DescendantsAndSelf("library_images") select item;

            scale = float.Parse(xdocument.Element("asset").Element("unit").Attribute("meter").Value);

            if (cameraNode.Count() > 0 && visualSceneNode.Count() > 0)
                ParseCamera(cameraNode, visualSceneNode); //OK
            ParseLights(lightNodes, visualSceneNode); //OK
            ParseMaterials(materialNodes, materialInfos, imageNodes);
            ParseGeometries(geometryNodes, visualSceneNode);
        }

        private void ParseLights(IEnumerable<XElement> lightNodes, IEnumerable<XElement> visualSceneNode)
        {
            foreach (var light in lightNodes.Elements("light"))
            {
                String id = light.Attribute("id").Value;
                String target = "";
                #region Directional Light
                if (light.Element("technique_common").Element("directional") != null)
                {
                    var lightNode = light.Element("technique_common").Element("directional");
                    float intensity = float.Parse(light.Element("technique").Element("intensity").Value);
                    Color color = ParseColor(lightNode.Element("color").Value).Mult(intensity);
                    Vector3 direction = Vector3.Zero;
                    foreach (var node in visualSceneNode.Elements("node"))
                    {
                        foreach (var lightInstance in node.Elements("instance_light"))
                        {
                            foreach (var technique in lightInstance.Parent.Element("extra").Elements("technique"))
                            {
                                if (technique.Element("target") != null)
                                    target = technique.Element("target").Value.Replace("#", "");
                            }
                        }
                        if (node.Attribute("name").Value.Equals(target))
                        {
                            if (node.Element("matrix") != null)
                            {
                                Vector4 dir = new Vector4(1, 1, 1, 1);
                                Vector4 res = ParseMatrix(node.Element("matrix").Value).Transform(dir);
                                direction = new Vector3(res.X, res.Y, res.Z);
                            }
                            else if (node.Element("translate") != null)
                            {
                                direction = ParseVector(node.Element("translate").Value);
                            }
                        }
                    }
                    Lights.Add(id, new DirectionalLight(direction, color));
                }
                #endregion
                #region Point Light
                if (light.Element("technique_common").Element("point") != null)
                {
                    var lightNode = light.Element("technique_common").Element("point");
                    float intensity = float.Parse(light.Element("technique").Element("intensity").Value);
                    Color color = ParseColor(lightNode.Element("color").Value).Mult(intensity);
                    Vector3 position = Vector3.Zero;
                    foreach (var node in visualSceneNode.Elements("node"))
                    {
                        foreach (var lightInstance in node.Elements("instance_light"))
                        {
                            if (lightInstance.Attribute("url").Value.Replace("#", "").Equals(id))
                            {
                                if (node.Element("matrix") != null)
                                {
                                    Vector4 dir = new Vector4(1, 1, 1, 1);
                                    Vector4 res = ParseMatrix(node.Element("matrix").Value).Transform(dir);

                                    position = new Vector3(res.X, res.Y, res.Z);
                                }
                                else if (node.Element("translate") != null)
                                {
                                    position = ParseVector(node.Element("translate").Value);
                                }

                            }
                        }
                    }
                    Lights.Add(id, new PointLight(position, color));
                }
                #endregion
            }
        }

        private void ParseGeometries(IEnumerable<XElement> geometryNodes, IEnumerable<XElement> visualSceneNodes)
        {

            foreach (var geometry in geometryNodes.Elements("geometry"))
            {
                String geometryId = geometry.Attribute("id").Value;
                Dictionary<String, float[]> floatArrays = new Dictionary<string, float[]>();
                Dictionary<String, String> positionToVertex = new Dictionary<string, string>();
                Dictionary<String, String> normalToVertex = new Dictionary<string, string>();

                //Get the transformation
                Matrix4 transformationMatrix = Matrix4.Identity;

                foreach (var node in visualSceneNodes.Elements("node"))
                {
                    if (node.Element("instance_geometry") != null && node.Element("instance_geometry").Attribute("url").Value.Replace("#", "").Equals(geometryId))
                    {
                        if (node.Element("matrix") != null)
                        {
                            transformationMatrix = ParseMatrix(node.Element("matrix").Value);
                            Matrix4 scaleMatrix = Matrix4.Scale(scale);
                            transformationMatrix = Matrix4.Mult(transformationMatrix, scaleMatrix);
                        }
                        else
                        {
                            Matrix4 rotationMatrix = Matrix4.Identity;
                            Matrix4 translationMatrix = Matrix4.Identity;
                            Matrix4 scaleMatrix = Matrix4.Identity;

                            //Rotation
                            translationMatrix = Matrix4.CreateRotationX((float)-Math.PI / 2);
                            foreach (var rotation in node.Elements("rotate"))
                            {
                                if (rotation.Attribute("sid").Value.Equals("rotateX"))
                                {
                                    Matrix4 rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(float.Parse(rotation.Value)));
                                    rotationMatrix = Matrix4.Mult(rotationMatrix, rotationX);
                                }
                                else if (rotation.Attribute("sid").Value.Equals("rotateY"))
                                {
                                    Matrix4 rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(float.Parse(rotation.Value)));
                                    rotationMatrix = Matrix4.Mult(rotationMatrix, rotationY);
                                }
                                else if (rotation.Attribute("sid").Value.Equals("rotateZ"))
                                {
                                    Matrix4 rotationZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(float.Parse(rotation.Value)));
                                    rotationMatrix = Matrix4.Mult(rotationMatrix, rotationZ);
                                }
                                transformationMatrix = Matrix4.Mult(transformationMatrix, rotationMatrix);
                            }
                            //Translation
                            if (node.Element("translation") != null)
                            {
                                transformationMatrix.Translation = new Vector4(ParseVector(node.Element("translation").Value));
                            }
                            //Scaling
                            if (node.Element("scale") != null)
                            {
                                scaleMatrix = Matrix4.CreateScaling(ParseVector(node.Element("scale").Value));
                                transformationMatrix = Matrix4.Mult(transformationMatrix, scaleMatrix);
                            }
                        }
                    }
                }

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
                            String positionId = element.Attribute("source").Value.Replace("#", "");
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
                        if (!string.IsNullOrEmpty(normalArrayName))
                        {
                            int.TryParse(normalArrayOffsets.First(), out normalOffset);
                            normals = floatArrays[normalArrayName];
                            count++;
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
                        if (!string.IsNullOrEmpty(normalArrayName))
                            normalIndices = ParseTriangleIndices(indices, normalOffset, count, triangleCount);
                        if (!string.IsNullOrEmpty(vertexArrayName))
                            vertexIndices = ParseTriangleIndices(indices, vertexOffset, count, triangleCount);
                        if (!string.IsNullOrEmpty(texCordArrayName))
                            texCoordIndices = ParseTriangleIndices(indices, texCoordOffset, count, triangleCount);
                        //Create the Mesh
                        Material mat;
                      
                        mat = Materials[materialName];


                        Mesh m = new Mesh(vertices, vertexIndices, normals, normalIndices, texCoords, texCoordIndices, mat);
                        BspAccelerator acc = new BspAccelerator();
                        acc.Construct(m);
                        Instance instance = new Instance(acc, transformationMatrix);
                        Meshes.Add(geometryId, instance);
                        i++;
                    }
                }

                #endregion


            }
        }

        private void ParseCamera(IEnumerable<XElement> cameraNode, IEnumerable<XElement> cameraInfos)
        {
            foreach (var camera in cameraNode.Descendants("camera"))
            {
                String id = camera.Attribute("id").Value;
                String target = "";
                float xfov, aspect;
                float.TryParse(camera.Element("optics").Element("technique_common").Element("perspective").Element("xfov").Value, out xfov);
                float.TryParse(camera.Element("optics").Element("technique_common").Element("perspective").Element("aspect_ratio").Value, out aspect);
                int width = 800;
                int height = (int)(width / aspect);
                //float yfov = xfov / aspect;

                Vector3 lookAt = Vector3.Zero;
                Vector3 eye = Vector3.Zero;
                Vector3 up = new Vector3(0, 1, 0);

                foreach (var node in cameraInfos.Elements("node"))
                {
                    foreach (var instanceCamera in node.Elements("instance_camera"))
                    {
                        if (instanceCamera.Attribute("url").Value.Replace("#", "").Equals(id))
                        {
                            if (instanceCamera.Parent.Element("matrix") != null)
                            {
                                Vector4 vec = new Vector4(0, 0, 0, 1);
                                Vector4 res = ParseMatrix(instanceCamera.Parent.Element("matrix").Value).Transform(vec);
                                eye = new Vector3(res.X, res.Y, res.Z);
                            }
                            else if (instanceCamera.Parent.Element("translate") != null)
                            {
                                eye = ParseVector(instanceCamera.Parent.Element("translate").Value);
                            }
                        }
                        foreach (var technique in instanceCamera.Parent.Element("extra").Elements("technique"))
                        {
                            target = technique.Value.Replace("#", "");
                        }
                    }
                    if (node.Attribute("name").Value.Equals(target))
                    {
                        if (node.Element("matrix") != null)
                        {
                            Vector4 vec = new Vector4(0, 0, 0, 1);
                            Vector4 res = ParseMatrix(node.Element("matrix").Value).Transform(vec);
                            lookAt = new Vector3(res.X, res.Y, res.Z);
                        }
                        else if (node.Element("translate") != null)
                        {
                            lookAt = ParseVector(node.Element("translate").Value);
                        }
                    }
                }

                Cameras.Add(id, new PinholeCamera() { Eye = eye, FieldOfView = xfov, LookAt = lookAt, AspectRation = aspect, ScreenHeight = height, ScreenWidth = width, Up = up});

            }

        }

        private void ParseMaterials(IEnumerable<XElement> materialNodes, IEnumerable<XElement> materialInfos, IEnumerable<XElement> imageNodes)
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
                String materialId = material.Attribute("id").Value;
                String materialName = material.Attribute("name").Value;

                //Determine the MaterialType
                foreach (var subNode in material.Element("profile_COMMON").Element("technique").Elements())
                {
                    if (subNode.Name == "lambert")
                    {
                        if (subNode.Element("diffuse").Element("texture") != null)
                        {
                            foreach (var image in imageNodes.Elements("image"))
                            {
                                if (image.Attribute("id").Value.Equals(subNode.Element("diffuse").Element("texture").Attribute("texture").Value))
                                {
                                    Texture texture = new Texture(image.Element("init_from").Value.Replace("file://", ""));
                                    Material mat = new LambertMaterial(new Color(0, 0, 0));
                                    mat.Texture = texture;
                                    Materials.Add(materialName, mat);
                                }
                            }
                        }
                        else
                        {
                            Color diffuse = ParseColor(subNode.Element("diffuse").Element("color").Value);
                            Material mat = new LambertMaterial(diffuse);
                            //Create lambert material
                            Materials.Add(materialName, mat);
                        }

                    }
                    if (subNode.Name == "blinn")
                    {
                        if (subNode.Element("diffuse").Element("texture") != null)
                        {
                            foreach (var image in imageNodes.Elements("image"))
                            {
                                if (image.Attribute("id").Value.Equals(subNode.Element("diffuse").Element("texture").Attribute("texture").Value))
                                {
                                    Texture texture = new Texture(image.Element("init_from").Value.Replace("file://", ""));
                                    Material mat = new LambertMaterial(new Color(0, 0, 0));
                                    mat.Texture = texture;
                                    Materials.Add(materialName, mat);
                                }
                            }
                        }
                        else
                        {
                            //Create BlinnPhong Material
                            Color diffuse = ParseColor(subNode.Element("diffuse").Element("color").Value);
                            Color specular = ParseColor(subNode.Element("specular").Element("color").Value);
                            float shininess = (float)Convert.ToDouble(subNode.Element("shininess").Element("float").Value);
                            Material mat = new BlinnPhongMaterial(diffuse, specular, shininess);
                            Materials.Add(materialName, mat);
                        }
                    }
                    if (subNode.Name == "phong")
                    {
                        if (subNode.Element("diffuse").Element("texture") != null)
                        {
                            foreach (var image in imageNodes.Elements("image"))
                            {
                                if (image.Attribute("id").Value.Equals(subNode.Element("diffuse").Element("texture").Attribute("texture").Value))
                                {
                                    Texture texture = new Texture(image.Element("init_from").Value.Replace("file://", ""));
                                    Material mat = new LambertMaterial(new Color(0, 0, 0));
                                    mat.Texture = texture;
                                    Materials.Add(materialName, mat);
                                }
                            }
                        }
                        else
                        {
                            float shininess = 0f;
                            //Create Refractive Material
                            Color diffuse = ParseColor(subNode.Element("diffuse").Element("color").Value);
                            Color specular = ParseColor(subNode.Element("specular").Element("color").Value);
                            try
                            {
                                shininess = (float)Convert.ToDouble(subNode.Element("shininess").Element("float").Value);
                            }
                            catch (Exception)
                            {
                            }
                            Material mat = new BlinnPhongMaterial(diffuse, specular, shininess);
                            Materials.Add(materialName, mat);
                        }
                    }

                }
            }
        }

        private Vector3 ParseVector(String vector)
        {
            String[] values = vector.Split(new char[] { ' ' });
            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }

        private Matrix4 ParseMatrix(string matrix)
        {
            String[] values = matrix.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
            Matrix4 m = Matrix4.Identity;
            m.M11 = float.Parse(values[0]);
            m.M12 = float.Parse(values[1]);
            m.M13 = float.Parse(values[2]);
            m.M14 = float.Parse(values[3]);
            m.M21 = float.Parse(values[4]);
            m.M22 = float.Parse(values[5]);
            m.M23 = float.Parse(values[6]);
            m.M24 = float.Parse(values[7]);
            m.M31 = float.Parse(values[8]);
            m.M32 = float.Parse(values[9]);
            m.M33 = float.Parse(values[10]);
            m.M34 = float.Parse(values[11]);
            m.M41 = float.Parse(values[12]);
            m.M42 = float.Parse(values[13]);
            m.M43 = float.Parse(values[14]);
            m.M44 = float.Parse(values[15]);

            return m;
        }

        private float[] ParseFloatArray(string values)
        {
            String[] sValues = values.Split(new char[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

        private Color ParseColor(String value)
        {
            String[] values = value.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);

            float r, g, b;

            float.TryParse(values[0], out r);
            float.TryParse(values[1], out g);
            float.TryParse(values[2], out b);
            return new Color(r, g, b);

        }
    }
}