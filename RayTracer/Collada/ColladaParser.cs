using System;
using System.Collections.Generic;
using RayTracer.SceneGraph;
using RayTracer.SceneGraph.Cameras;
using RayTracer.SceneGraph.Light;
using RayTracer.SceneGraph.Materials;
using RayTracer.SceneGraph.Objects;
using RayTracer.Structs;

namespace RayTracer.Collada
{
    public class ColladaParser
    {
        public ICamera Camera { get; private set; }
        public List<Mesh> Meshes { get; private set; }
        public List<ILight> Lights { get; private set; }

        public void ParseColladaFile(String filename)
        {

            Meshes = new List<Mesh>();
            Lights = new List<ILight>();

            COLLADA model = COLLADA.Load(filename);
            // Iterate on libraries
            foreach (var item in model.Items)
            {
                var geometries = item as library_geometries;
                if (geometries == null)
                    continue;

                float[] myVertices = null;
                // Iterate on geomerty in library_geometries 
                foreach (var geom in geometries.geometry)
                {
                    var mesh = geom.Item as mesh;
                    if (mesh == null)
                        continue;
                    // Dump source[] for geom
                    foreach (var source in mesh.source)
                    {
                        var float_array = source.Item as float_array;
                        if (float_array == null)
                            continue;

                        Console.Write("Geometry {0} source {1} : ", geom.id, source.id);
                        myVertices = new float[float_array.count];
                        int i = 0;
                        
                        foreach (var mesh_source_value in float_array.Values)
                        {
                            myVertices[i] = (float)mesh_source_value;
                            i++;
                        }
                    }
                    List<int> indicesList = null;
                    // Dump Items[] for geom
                    foreach (var meshItem in mesh.Items)
                    {

                        if (meshItem is vertices)
                        {
                            var vertices = meshItem as vertices;
                            var inputs = vertices.input;
                            foreach (var input in inputs)
                                Console.WriteLine("\t Semantic {0} Source {1}", input.semantic, input.source);
                        }
                        else if (meshItem is triangles)
                        {
                            var triangles = meshItem as triangles;
                            var inputs = triangles.input;
                           
                            foreach (var input in inputs)
                            {
                                char[] whitespace = new char[] { ' '};
                                String[] strIndices = triangles.p.Split(whitespace);
                                
                                indicesList = new List<int>();
                                foreach (string strIndex in strIndices)
                                {
                                    if (!String.IsNullOrEmpty(strIndex))
                                    {
                                        indicesList.Add(Convert.ToInt32(strIndex));
                                    }
                                }
                                Console.WriteLine("\t Semantic {0} Source {1} Offset {2}", input.semantic, input.source, input.offset);
                            }
                        }
                    }
                    Mesh myMesh = new Mesh(new LambertMaterial(new Color(0.5f, 0.5f, 0.5f)), indicesList.ToArray(), myVertices);
                    Meshes.Add(myMesh);
                }
            }
        }

        private void ParseCamera()
        {
        }
    }
}