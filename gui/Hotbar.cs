using EminaCraft.entities;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft.gui
{
    class Hotbar : GUIObject
    {
        private MouseState mouse;
        private Player player;
        private Inventory inventory;
        private GUIText numText;
        public Hotbar(MouseState mouse, Player player, Inventory inventory) : base(Vector2.Zero, Vector2.One/2f, Toolbox.textures["hotbar"])
        {
            numSprites = 8;
            curSprite = 0;
            this.mouse = mouse;
            this.player = player;
            this.inventory = inventory;
            numText = new GUIText(Vector2.Zero, 0.25f, Fonts.FONT_SMALL, "");
        }
        public override void Render(RawModel square, GUIShader shader)
        {
            for(int i = 0; i < 8; i++)
            {
                curSprite = inventory.selectedItem == i ? 1 : 0;
                size = Vector2.One * 0.5f;
                position = new Vector2(i / 2f - 2f, -3.4f);
                Render(square, shader,position);

                curSprite = (i+1) * 2 < player.health+1 ? 2 : ((i+1) * 2 == player.health+1 ? 3 : 4);
                size = Vector2.One * 0.3f;
                Render(square, shader, new Vector2(i * 0.3f - 2.1f, -2.6f));
                
                ItemStack? item = inventory.items[i];
                if (item != null)
                {
                    item.item.texture.bind(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
                    shader.position = position;
                    shader.scale = Vector2.One * 0.3f;
                    shader.uploadUniforms();
                    OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip, 0, square.vertexCount);
                    numText.position = position + Vector2.One * 0.2f;

                    numText.Text = "" + item.amount;
                    numText.Render(square, shader);
                }
            }
        }
        public override void Update()
        {
            int scrollDelta = -(int)mouse.ScrollDelta.Y;
            inventory.selectedItem = (inventory.selectedItem + scrollDelta) % 8;
            if (inventory.selectedItem < 0) inventory.selectedItem = 7;
        }
    }
}
