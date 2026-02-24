using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;

namespace KonradCraft.gui
{
    internal class GUIText : GUIObject
    {

        public string Text
        {
            set
            {
                _text.Clear();
                currentText = value;
                for (int i = 0; i < value.Length; i++)
                {
                    _text.Add(font.fromChar(value[i]));
                }
            }
        }
        private List<int> _text = new List<int>();
        private Font font;
        public string currentText = "";
        public GUIText(Vector2 position, float size, Font font, string text) : base(position, new Vector2(size * font.width, size), font.texture)
        {
            this.font = font;
            Text = text;
            numSprites = font.characters;
        }
        public override void Render(RawModel square, GUIShader shader)
        {
            texture.bind(TextureUnit.Texture0);
            int textures = _text.Count;
            int curXPos = 0;
            int curYPos = 0;
            for (int i = 0; i < textures; i++)
            {
                curSprite = _text[i];
                if (curSprite == -1)
                {
                    curYPos += 1;
                    curXPos = 0;
                    continue;
                }
                shader.position = new Vector2(position.X + size.X * curXPos, position.Y - curYPos * size.Y * 2f);
                curXPos++;
                if (curSprite == 0) continue;
                shader.curSprite = curSprite;
                shader.color = color;
                shader.numSprites = numSprites;
                shader.scale = size;
                shader.uploadUniforms();
                DrawArrays(PrimitiveType.TriangleStrip, 0, square.vertexCount);
            }
        }
        public override void Render(RawModel square, GUIShader shader, Vector2 pos)
        {
            texture.bind(TextureUnit.Texture0);
            int textures = _text.Count;
            int curXPos = 0;
            int curYPos = 0;
            for (int i = 0; i < textures; i++)
            {
                curSprite = _text[i];
                if (curSprite == -1)
                {
                    curYPos += 1;
                    curXPos = 0;
                    continue;
                }
                shader.position = new Vector2(pos.X + size.X * curXPos, pos.Y - curYPos * size.Y * 2f);
                curXPos++;
                if (curSprite == 0) continue;
                shader.curSprite = curSprite;
                shader.color = color;
                shader.numSprites = numSprites;
                shader.scale = size;
                shader.uploadUniforms();
                DrawArrays(PrimitiveType.TriangleStrip, 0, square.vertexCount);
            }
        }

        public void AddChar(char ch)
        {
            _text.Add(font.fromChar(ch));
            currentText += ch;
        }

    }
}
