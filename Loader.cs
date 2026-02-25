using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EminaCraft
{
    internal class Loader
    {
        public static RawModel loadToVAO(float[] vertices, int dimensions)
        {
            int vboID = GenBuffer();

            BindBuffer(BufferTarget.ArrayBuffer, vboID);
            BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ArrayBuffer, 0);

            int vaoID = GenVertexArray();
            BindVertexArray(vaoID);

            BindBuffer(BufferTarget.ArrayBuffer, vboID);

            //POSITION
            VertexAttribPointer(0, dimensions, VertexAttribPointerType.Float, false, dimensions * sizeof(float), 0);

            EnableVertexAttribArray(0);

            //Unbind
            BindVertexArray(0);

            return new RawModel(vaoID, vertices.Length / dimensions);

        }
        public static Model loadToVAO(float[] vertices, int[] indices, int[] textures)
        {
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


            //Creating the VAO itself, each vertex is X,Y,Z,NX,NY,NZ,U,V
            vertexArrayHandle = GenVertexArray();
            BindVertexArray(vertexArrayHandle);

            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //POSITION
            VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            //NORMAL
            VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            //UV
            VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            EnableVertexAttribArray(0);
            EnableVertexAttribArray(1);
            EnableVertexAttribArray(2);

            //Unbind
            BindVertexArray(0);

            return new Model(vertexArrayHandle, vertexBufferHandle, indexBufferHandle, vertices.Length, indices.Length, textures);
        }
        public static void loadToExistingVAO(float[] vertices, int[] indices, SimpleModel model)
        {
            if (vertices == null || indices == null)    //Exit if the data is missing
            {
                Environment.Exit(0);
            }

            BindBuffer(BufferTarget.ArrayBuffer, model.vboHandle);

            //BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), IntPtr.Zero, BufferUsageHint.StaticDraw); // Orphan the buffer
            BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ArrayBuffer, 0);

            BindBuffer(BufferTarget.ElementArrayBuffer, model.indexHandle);
            
            //BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), IntPtr.Zero, BufferUsageHint.StaticDraw); // Orphan the buffer
            BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            model.vertexCount = vertices.Length;
            model.indicesCount = indices.Length;
        }
        /*public static void loadToExistingVAO(float[] vertices, int[] indices, SimpleModel model)
        {
            BindVertexArray(0);
            DeleteVertexArray(model.vaoHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            BindBuffer(BufferTarget.ArrayBuffer, 0);
            DeleteBuffer(model.indexHandle);
            DeleteBuffer(model.vboHandle);

            SimpleModel newModel = loadToVAO(vertices, indices);
            model.vaoHandle = newModel.vaoHandle;
            model.vboHandle = newModel.vboHandle;
            model. 

        }*/
        public static SimpleModel loadToVAO(float[] vertices, int[] indices)
        {
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


            //Creating the VAO itself, each vertex is X,Y,Z,NX,NY,NZ,U,V
            vertexArrayHandle = GenVertexArray();
            BindVertexArray(vertexArrayHandle);

            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //POSITION
            VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            //NORMAL
            VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            //UV
            VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            EnableVertexAttribArray(0);
            EnableVertexAttribArray(1);
            EnableVertexAttribArray(2);

            //Unbind
            BindVertexArray(0);

            return new SimpleModel(vertexArrayHandle, vertexBufferHandle, indexBufferHandle, vertices.Length, indices.Length);
        }
        public static RenderableRawModel loadToVAO(float[] vertices)
        {
            int vertexBufferHandle;
            int vertexArrayHandle;

            if (vertices == null)    //Exit if the data is missing
            {
                Environment.Exit(0);
            }

            //Creating the VBO for vertices
            vertexBufferHandle = GenBuffer();
            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            BindBuffer(BufferTarget.ArrayBuffer, 0);

            //Creating the VAO itself, each vertex is X,Y,Z,NX,NY,NZ,U,V
            vertexArrayHandle = GenVertexArray();
            BindVertexArray(vertexArrayHandle);

            BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
            //POSITION
            VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            //NORMAL
            VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            //UV
            VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            EnableVertexAttribArray(0);
            EnableVertexAttribArray(1);
            EnableVertexAttribArray(2);

            //Unbind
            BindVertexArray(0);

            return new RenderableRawModel(vertexArrayHandle, vertices.Length);
        }
    }
}
