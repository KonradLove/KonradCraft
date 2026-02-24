using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;

namespace KonradCraft.gui
{
    internal class GUIObject
    {
        public Vector2 position;
        public Vector2 size;
        public Texture texture;
        public Color4 color;
        public int numSprites;
        public bool visible = true;
        public int curSprite;
        public GUIObject(Vector2 position, Vector2 size, Texture texture)
        {
            this.position = position;
            this.size = size;
            this.texture = texture;
            numSprites = 1;
            color = OpenTK.Mathematics.Color4.White;
        }
        public GUIObject(Vector2 position, Vector2 size, Texture texture, int numSprites)
        {
            this.position = position;
            this.size = size;
            this.texture = texture;
            this.numSprites = numSprites;
            color = OpenTK.Mathematics.Color4.White;
        }

        public bool inBounds(Vector2 point)
        {
            Vector2 pos = point;
            pos *= 4f;
            float aspect = 1080f / 1920f;
            if (pos.X > position.X - size.X * aspect && pos.X < position.X + size.X * aspect)
            {
                if (pos.Y > position.Y - size.Y && pos.Y < position.Y + size.Y)
                {
                    return true;
                }
            }
            return false;
        }
        public virtual void Update()
        {

        }
        public virtual void Render(RawModel square, GUIShader shader)
        {
            texture.bind(TextureUnit.Texture0);
            shader.uploadValues(this);
            shader.uploadUniforms();
            DrawArrays(PrimitiveType.TriangleStrip, 0, square.vertexCount);
        }
        public virtual void Render(RawModel square, GUIShader shader, Vector2 pos)
        {
            texture.bind(TextureUnit.Texture0);
            shader.uploadValues(this);
            shader.position = pos;
            shader.uploadUniforms();
            DrawArrays(PrimitiveType.TriangleStrip, 0, square.vertexCount);
        }
    }
}
