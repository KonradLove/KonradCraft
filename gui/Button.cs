using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace EminaCraft.gui
{
    interface ButtonRunnable
    {
        public void Run();
    }
    internal class Button : GUIObject
    {
        private string text;
        private MouseState mouse;
        private Color4 normalColor;
        private GUIText guiText;
        private Action action;
        public Button(Vector2 position, Vector2 scale, string text, MouseState mouse, Color4 color, Action action) : base(position, scale, Toolbox.textures["button"])
        {
            this.text = text;
            this.mouse = mouse;
            normalColor = color;
            this.action = action;
            guiText = new GUIText(new Vector2(position.X - (text.Length - 1) * 0.15f, position.Y), 0.3f, Fonts.FONT_LARGE, text);
        }
        public Button(Vector2 position, Vector2 scale, string text, MouseState mouse, Color4 color, Action action, Font font) : base(position, scale, Toolbox.textures["button"])
        {
            this.text = text;
            this.mouse = mouse;
            normalColor = color;
            this.action = action;
            guiText = new GUIText(new Vector2(position.X - (text.Length - 1) * 0.15f, position.Y), 0.3f, font, text);
        }
        bool hovering = false;
        public override void Render(RawModel square, GUIShader shader)
        {
            color = hovering ? (Color4)((Vector4)normalColor * 2f) : normalColor;
            base.Render(square, shader);
            guiText.Render(square, shader);

        }
        public override void Update()
        {
            base.Update();
            guiText.Update();
            hovering = inBounds(MouseTools.ScreenToWorld(mouse.Position));
            if (hovering && mouse.IsButtonReleased(MouseButton.Left))
            {
                action();
            }
        }

    }
}
