using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Graphics.OpenGL.GL;

namespace EminaCraft
{
    internal class TexturedModel : IRenderable
    {
        //The model and texture we are connecting in one class
        public  Model model;
        private Texture texture;
        public TexturedModel(Model model, Texture texture)
        {
            this.model = model;
            this.texture = texture;
        }
        public void Bind()
        {
            model.Bind();
            texture.bind(TextureUnit.Texture0);
        }
        public void Unbind() {
            model.Unbind();
            texture.unbind(TextureUnit.Texture0);
        }
        public void Render()
        {
            model.Render();
        }
        //acab even the memory police
        public void CleanUp()
        {
            model.CleanUp();
        }
    }
    internal class RawModel
    {
        public int vaoID;
        public int vertexCount;

        public RawModel(int vaoID, int vertexCount)
        {
            this.vaoID = vaoID;
            this.vertexCount = vertexCount;
        }
    }
    internal class RenderableRawModel : RawModel, IRenderable
    {
        public RenderableRawModel(int vaoID, int vertexCount) : base(vaoID, vertexCount)
        {
        }

        //Cleans up memory when program ends
        public void CleanUp()
        {
            BindVertexArray(0);
            DeleteVertexArray(vaoID);
        }
        //Binds the model to the GPU before being rendered
        public void Bind()
        {

        }
        //Unbinds after being rendered
        public void Unbind()
        {


        }
        //does the actual rendering thing
        public void Render()
        {
            BindVertexArray(vaoID);
            EnableVertexAttribArray(0);
            Enable(EnableCap.Blend);
            BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Disable(EnableCap.DepthTest);

            DrawArrays(PrimitiveType.Triangles, 0, vertexCount);

            Disable(EnableCap.Blend);
            Enable(EnableCap.DepthTest);
            DisableVertexAttribArray(0);
            BindVertexArray(0);

        }
    }
    internal class EmptyRenderable : IRenderable
    {
        public void Bind()
        {
        }

        public void CleanUp()
        {
        }

        public void Render()
        {
        }

        public void Unbind()
        {
        }
    }
    internal class SimpleModel : IRenderable
    {
        //Information about the model on the GPU
        public int vaoHandle;
        public int vboHandle;
        public int indexHandle;
        public int vertexCount;
        public int indicesCount;
        public SimpleModel(int vaoHandle, int vboHandle, int indexHandle, int vertexCount, int indicesCount)
        {
            this.vboHandle = vboHandle;
            this.indexHandle = indexHandle;
            this.vaoHandle = vaoHandle;
            this.vertexCount = vertexCount;
            this.indicesCount = indicesCount;
        }

        //Cleans up memory when program ends
        public void CleanUp()
        {
            BindVertexArray(0);
            DeleteVertexArray(vaoHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            BindBuffer(BufferTarget.ArrayBuffer, 0);
            DeleteBuffer(indexHandle);
            DeleteBuffer(vboHandle);
        }
        //Binds the model to the GPU before being rendered
        public void Bind()
        {
            BindVertexArray(vaoHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, indexHandle);
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
            DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
        public void RenderSection(int start, int length)
        {
            DrawElements(PrimitiveType.Triangles, length * 3, DrawElementsType.UnsignedInt, start * 3 * sizeof(int));
        }
    }
    internal class Model : IRenderable 
    {
        //Information about the model on the GPU
        public int vaoHandle { get; }
        public int vboHandle { get; }
        public int indexHandle { get; }
        public int vertexCount { get; }
        public int indicesCount { get; }
        public int[] textures;
        public Model(int vaoHandle, int vboHandle, int indexHandle, int vertexCount, int indicesCount, int[] textures)
        {
            this.vboHandle = vboHandle;
            this.indexHandle = indexHandle;
            this.vaoHandle = vaoHandle;
            this.vertexCount = vertexCount;
            this.indicesCount = indicesCount;
            this.textures = textures;
        }
       
        //Cleans up memory when program ends
        public void CleanUp()
        {
            BindVertexArray(0);
            DeleteVertexArray(vaoHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            BindBuffer(BufferTarget.ArrayBuffer, 0);
            DeleteBuffer(indexHandle);
            DeleteBuffer(vboHandle);
        }
        //Binds the model to the GPU before being rendered
        public void Bind()
        {
            BindVertexArray(vaoHandle);
            BindBuffer(BufferTarget.ElementArrayBuffer, indexHandle);
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
            DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
        }
        public void RenderSection(int start, int length)
        {
            DrawElements(PrimitiveType.Triangles, length*3, DrawElementsType.UnsignedInt, start*3*sizeof(int));
        }
    }
}