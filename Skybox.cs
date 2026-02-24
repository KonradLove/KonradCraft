using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace KonradCraft
{
    internal class Skybox
    {
        private int textureID;
        public float size = 1000f;
        private SkyboxShader shader;
        private Model model;
        private Cubemap cubemap;
        public Color4 color;
        public Skybox(string name, Camera cam, Model model)
        {
            this.model = model;
            shader = new SkyboxShader(cam);
            cubemap = new Cubemap(name);
            color = Color4.White;
        }
        public Skybox(string name, Camera cam, Model model, int num)
        {
            this.model = model;
            shader = new SkyboxShader(cam);
            cubemap = new Cubemap(name, num);
            color = Color4.White;
        }
        public Skybox(string name, Camera cam, Model model, Color4 color)
        {
            this.model = model;
            this.color = color;
            shader = new SkyboxShader(cam);
            cubemap = new Cubemap(name);
        }
        public void Render()
        {
            shader.bind();

            shader.currentSkybox = this;
            shader.uploadUniforms();

            model.Bind();

            cubemap.Bind();

            model.Render();
            model.Unbind();

            cubemap.Unbind();

            shader.unbind();
        }

        public void cleanUp()
        {
            shader.cleanUp();
        }
    }
}
