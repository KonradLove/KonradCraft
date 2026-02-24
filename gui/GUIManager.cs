using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;

namespace KonradCraft.gui
{
    internal class GUIManager
    {
        public List<GUIObject> objects = new List<GUIObject>();
        private RawModel square;
        private GUIShader shader;
        public GUIObject Add(GUIObject obj)
        {
            objects.Add(obj);
            return obj;
        }
        public GUIManager(float aspectRatio)
        {
            square = Toolbox.square;
            shader = new GUIShader(aspectRatio);
        }

        public void Render()
        {
            shader.bind();
            BindVertexArray(square.vaoID);
            EnableVertexAttribArray(0);
            Enable(EnableCap.Blend);
            BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Disable(EnableCap.DepthTest);
            foreach (GUIObject obj in objects)
            {
                if (obj.visible)
                    obj.Render(square, shader);
            }
            Disable(EnableCap.Blend);
            Enable(EnableCap.DepthTest);
            DisableVertexAttribArray(0);
            BindVertexArray(0);
            shader.unbind();
        }
        public void cleanUp()
        {
            shader.cleanUp();
        }
        public void Update()
        {
            foreach (GUIObject gui in objects)
            {
                gui.Update();
            }
        }
    }
}
