using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft
{
    internal class MultitextureModel : IRenderable
    {
        public Texture[] textures;
        public Model model;

        public MultitextureModel(Texture[] textures, Model model)
        {
            this.textures = textures;
            this.model = model;
        }

        public void Bind()
        {
            model.Bind();
        }

        public void CleanUp()
        {
            model.CleanUp();
            foreach (var texture in textures)
            {
                texture.cleanUp();
            }
        }

        public void Render()
        {
            //Start at the beginning of the model
            int start = 0;
            for (int i = 0; i < model.textures.Length; i++)
            {
                //How many triangles have this texture?
                int texLength = model.textures[i];
                //Bind the texture
                textures[i].bind(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
                //Render the triangles used by this texture, they are in order luckily
                model.RenderSection(start, texLength);
                textures[i].unbind(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
                //Go to the triangle with the next texture!
                start += texLength;
            }

        }

        public void Unbind()
        {
            model.Unbind();
        }
    }
}
