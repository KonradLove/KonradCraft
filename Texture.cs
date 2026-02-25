using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using SFML.Graphics;

namespace EminaCraft
{
    internal class Cubemap
    {
        private int textureID;
        public Cubemap(string name, int num)
        {
            textureID = GenTexture();
            ActiveTexture(TextureUnit.Texture0);
            BindTexture(TextureTarget.TextureCubeMap, textureID);

            for (int i = 0; i < 6; i++)
            {
                Image image = new Image("./res/textures/skybox/" + name + ".png");

                TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                    (int)image.Size.X, (int)image.Size.Y, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, image.Pixels);
            }

            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            BindTexture(TextureTarget.TextureCubeMap, 0);
        }
        public Cubemap(string name)
        {
            textureID = GenTexture();
            ActiveTexture(TextureUnit.Texture0);
            BindTexture(TextureTarget.TextureCubeMap, textureID);

            for (int i = 0; i < 6; i++)
            {
                Image image = new Image("./res/textures/skybox/" + name + i + ".png");

                TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba,
                    (int)image.Size.X, (int)image.Size.Y, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, image.Pixels);
            }

            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            BindTexture(TextureTarget.TextureCubeMap, 0);
        }
        public void Bind()
        {
            BindTexture(TextureTarget.TextureCubeMap, textureID);
        }

        public void Unbind()
        {
            BindTexture(TextureTarget.TextureCubeMap, 0);
        }
    }
    internal class Texture
    {
        private int textureHandle;
        public string name;

        public Texture(string name, TextureMinFilter filter)
        {
            this.name = name;
            //Load the image using SFML
            Image image = new Image("./res/textures/" + name + ".png");
            //Generate a texture on the GPU and retrieve the handle
            textureHandle = GenTexture();
            BindTexture(TextureTarget.Texture2D, textureHandle);

            TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                (int)image.Size.X, (int)image.Size.Y, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, image.Pixels
            );

            //Behaviour when scaling, it will now interpolate between pixels.
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);

            //The texture will now wrap
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);


            //unbind after we are finished of course!
            BindTexture(TextureTarget.Texture2D, 0);
        }

        //Bind for use
        public void bind(TextureUnit unit)
        {
            ActiveTexture(unit);
            BindTexture(TextureTarget.Texture2D, textureHandle);
        }
        //unbind after!
        public void unbind(TextureUnit unit)
        {
            ActiveTexture(unit);
            BindTexture(TextureTarget.Texture2D, 0);
        }

        //acab even the memory police
        public void cleanUp()
        {
            DeleteTexture(textureHandle);
        }
    }
}
