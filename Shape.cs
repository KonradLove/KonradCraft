using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Graphics.OpenGL.GL;

namespace KonradCraft
{
    public class Shape
    {
        /*public static Shape SQUARE = new Shape(
            new float[]
            {
            //  POSITION    NORMAL  UV COORD
                -1, 1, 0f,  1,1,1,   0f, 0f,
                 1, 1, 0f,  1,1,1,   1f, 0f,
                 1,-1, 0f,  1,1,1,   1f, 1f,
                -1,-1, 0f,  1,1,1,   0f, 1f,
            },
            new int[]
            {
                0,1,2,
                0,2,3
            }
        );*/
        //Data from https://pastebin.com/57srK1a2
        public static Shape CUBE = new Shape(
            new float[]
            {
                -0.5f, -0.5f, -0.5f,  // A 0
                0.5f, -0.5f, -0.5f,   // B 1
                0.5f,  0.5f, -0.5f,   // C 2
                -0.5f,  0.5f, -0.5f,  // D 3
                -0.5f, -0.5f,  0.5f,  // E 4
                0.5f, -0.5f,  0.5f,   // F 5
                0.5f,  0.5f,  0.5f,   // G 6
                -0.5f,  0.5f,  0.5f,  // H 7
 
                -0.5f,  0.5f, -0.5f,  // D 8
                -0.5f, -0.5f, -0.5f,  // A 9
                -0.5f, -0.5f,  0.5f,  // E 10
                -0.5f,  0.5f,  0.5f,  // H 11
                0.5f, -0.5f, -0.5f,   // B 12
                0.5f,  0.5f, -0.5f,   // C 13
                0.5f,  0.5f,  0.5f,   // G 14
                0.5f, -0.5f,  0.5f,   // F 15
 
                -0.5f, -0.5f, -0.5f,  // A 16
                0.5f, -0.5f, -0.5f,   // B 17
                0.5f, -0.5f,  0.5f,   // F 18
                -0.5f, -0.5f,  0.5f,  // E 19
                0.5f,  0.5f, -0.5f,   // C 20
                -0.5f,  0.5f, -0.5f,  // D 21
                -0.5f,  0.5f,  0.5f,  // H 22
                0.5f,  0.5f,  0.5f
            },
            new int[]
            {
                // front and back
                0, 3, 2,
                2, 1, 0,
                4, 5, 6,
                6, 7 ,4,
                // left and right
                11, 8, 9,
                9, 10, 11,
                12, 13, 14,
                14, 15, 12,
                // bottom and top
                16, 17, 18,
                18, 19, 16,
                20, 21, 22,
                22, 23, 20
            }
        );
        private int vertexArrayHandle, vertexBufferHandle, indexBufferHandle;
        public float[] vertices { get; }
        public int[] indices { get; }
        public Shape(float[] vertices, int[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            int vertexBufferHandle, indexBufferHandle;
            int vertexArrayHandle;

            if (vertices == null || indices == null)    //Exit if the data is missing
            {
                Environment.Exit(0);
            }

            //Creating the VBO for vertices
            vertexBufferHandle = GenBuffer();
            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Creates the buffer for indices of the model
            indexBufferHandle = GenBuffer();
            BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
            BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            //Creating the VAO itself, each vertex is X,Y,Z
            vertexArrayHandle = GenVertexArray();
            BindVertexArray(vertexArrayHandle);

            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //POSITION
            VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            EnableVertexAttribArray(0);

            //Unbind
            BindVertexArray(0);
        }
        //Cleans up memory when program ends
        public void CleanUp()
        {
            BindVertexArray(0);
            DeleteVertexArray(vertexArrayHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            BindBuffer(BufferTarget.ArrayBuffer, 0);
            DeleteBuffer(indexBufferHandle);
            DeleteBuffer(vertexBufferHandle);
        }
        //Binds the model to the GPU before being rendered
        public void Bind()
        {
            BindVertexArray(vertexArrayHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
        }
        //Unbinds after being rendered
        public void Unbind()
        {
            BindVertexArray(0);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            BindBuffer(BufferTarget.ArrayBuffer, 0);

        }
        //does the actual rendering thing
        public void Render()
        {
            DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
