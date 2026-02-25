using OpenTK.Mathematics;

namespace EminaCraft
{
    class Vertex
    {
        private const int NO_INDEX = -1;

        private Vector3 position;
        private int textureIndex = NO_INDEX;
        private int normalIndex = NO_INDEX;
        private Vertex? duplicateVertex = null;
        private int index;

        public Vertex(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
        }

        public int getIndex()
        {
            return index;
        }

        public bool isSet()
        {
            return textureIndex != NO_INDEX && normalIndex != NO_INDEX;
        }

        public bool hasSameTextureAndNormal(int textureIndexOther, int normalIndexOther)
        {
            return textureIndexOther == textureIndex && normalIndexOther == normalIndex;
        }

        public void setTextureIndex(int textureIndex)
        {
            this.textureIndex = textureIndex;
        }

        public void setNormalIndex(int normalIndex)
        {
            this.normalIndex = normalIndex;
        }

        public Vector3 getPosition()
        {
            return position;
        }

        public int getTextureIndex()
        {
            return textureIndex;
        }

        public int getNormalIndex()
        {
            return normalIndex;
        }

        public Vertex? getDuplicateVertex()
        {
            return duplicateVertex;
        }

        public void setDuplicateVertex(Vertex duplicateVertex)
        {
            this.duplicateVertex = duplicateVertex;
        }
    }
    internal class OBJLoader
    {
        public static Model loadOBJ(string objFileName)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines("./res/models/" + objFileName + ".obj");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("File not found in res; don't use any extention");
                Environment.Exit(1);
                return null;
            }
            string? line;
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Triangle> triangles = new List<Triangle>();
            int cLine = 0;

            List<int> textureIDs = new List<int>();
            try
            {
                while (true)
                {

                    line = lines[cLine++];
                    if (line.StartsWith("v "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 vertex = new Vector3((float)float.Parse(currentLine[1]),
                                (float)float.Parse(currentLine[2]),
                                (float)float.Parse(currentLine[3]));
                        Vertex newVertex = new Vertex(vertices.Count, vertex);
                        vertices.Add(newVertex);
                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector2 texture = new Vector2((float)float.Parse(currentLine[1]),
                                (float)float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 normal = new Vector3((float)float.Parse(currentLine[1]),
                                (float)float.Parse(currentLine[2]),
                                (float)float.Parse(currentLine[3]));
                        normals.Add(normal);
                    }
                    else if (line.StartsWith("f "))
                    {
                        break;
                    }
                }
                int materialCounter = 0;
                while (line != null && (line.StartsWith("f ") || line.StartsWith("usemtl ")))
                {
                    //Not a material line
                    if (!line.StartsWith("usemtl"))
                    {

                        string[] currentLine = line.Split(" ");

                        //All 3 vertices and their associated data
                        string[] vertex1 = currentLine[1].Split("/");
                        string[] vertex2 = currentLine[2].Split("/");
                        string[] vertex3 = currentLine[3].Split("/");

                        //Send them in for processing
                        Vertex v1 = processVertex(vertex1, vertices, indices);
                        Vertex v2 = processVertex(vertex2, vertices, indices);
                        Vertex v3 = processVertex(vertex3, vertices, indices);

                        triangles.Add(new Triangle(v1.getPosition(), v2.getPosition(), v3.getPosition(),
                            (normals[v1.getNormalIndex()] + normals[v2.getNormalIndex()] +
                            normals[v3.getNormalIndex()]) / 3f, materialCounter));

                        //Increment the material counter!
                        materialCounter++;
                    }
                    else
                    {
                        textureIDs.Add(materialCounter);
                        materialCounter = 0;
                    }
                    
                    if (cLine == lines.Length)
                        line = null;
                    else
                        line = lines[cLine++];
                }
                textureIDs.Add(materialCounter);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error reading the file");
            }
            removeUnusedVertices(vertices);


            int[] indicesArray = convertIndicesListToArray(indices);
            float[] verticesOut = new float[vertices.Count * 8];
            Vector3 smallestPos = Vector3.PositiveInfinity, largestPos = Vector3.NegativeInfinity;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];

                Vector3 position = currentVertex.getPosition();
                Vector2 textureCoord = textures[currentVertex.getTextureIndex()];
                Vector3 normalVector = normals[currentVertex.getNormalIndex()];

                if (position.X < smallestPos.X) smallestPos.X = position.X;
                if(position.Y < smallestPos.Y) smallestPos.Y = position.Y;
                if( position.Z < smallestPos.Z) smallestPos.Z = position.Z;

                if(position.X > largestPos.X) largestPos.X = position.X;
                if (position.Y > largestPos.Y) largestPos.Y = position.Y;
                if (position.Z > largestPos.Z) largestPos.Z = position.Z;

                verticesOut[i * 8] = position.X;
                verticesOut[i * 8 + 1] = position.Y;
                verticesOut[i * 8 + 2] = position.Z;

                verticesOut[i * 8 + 3] = normalVector.X;
                verticesOut[i * 8 + 4] = normalVector.Y;
                verticesOut[i * 8 + 5] = normalVector.Z;

                verticesOut[i * 8 + 6] = textureCoord.X;
                verticesOut[i * 8 + 7] = 1 - textureCoord.Y;
            }
            Model model = Loader.loadToVAO(verticesOut, indicesArray, textureIDs.ToArray());
            return model;

        }
        
        private static Vertex processVertex(string[] vertex, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertex[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;
            if (!currentVertex.isSet())
            {
                currentVertex.setTextureIndex(textureIndex);
                currentVertex.setNormalIndex(normalIndex);
                indices.Add(index);
            }
            else
            {
                dealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices,
                        vertices);
            }
            return currentVertex;
        }

        private static int[] convertIndicesListToArray(List<int> indices)
        {
            int[] indicesArray = new int[indices.Count];
            for (int i = 0; i < indicesArray.Length; i++)
            {
                indicesArray[i] = indices[i];
            }
            return indicesArray;
        }



        private static void dealWithAlreadyProcessedVertex(Vertex previousVertex, int newTextureIndex,
                int newNormalIndex, List<int> indices, List<Vertex> vertices)
        {
            if (previousVertex.hasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertex.getIndex());
            }
            else
            {
                Vertex? anotherVertex = previousVertex.getDuplicateVertex();
                if (anotherVertex != null)
                {
                    dealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex,
                            indices, vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.getPosition());
                    duplicateVertex.setTextureIndex(newTextureIndex);
                    duplicateVertex.setNormalIndex(newNormalIndex);
                    previousVertex.setDuplicateVertex(duplicateVertex);
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.getIndex());
                }

            }
        }

        private static void removeUnusedVertices(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                if (!vertex.isSet())
                {
                    vertex.setTextureIndex(0);
                    vertex.setNormalIndex(0);
                }
            }
        }
    }
}
