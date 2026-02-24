using KonradCraft.gui;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Text.Json.Serialization;
using System.Text.Json;
using KonradCraft.shaders;

namespace KonradCraft
{
    internal class Toolbox
    {
        public static Dictionary<string, IRenderable> models = new Dictionary<string, IRenderable>();
        public static Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        public static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        public static Camera camera;
        public static RawModel square;

        public static void initToolbox()
        {
            square = Loader.loadToVAO(new float[] { -1, 1, -1, -1, 1, 1, 1, -1 }, 2);

            textures.Add("blocks", new Texture("Blocks", TextureMinFilter.Nearest));
            textures.Add("button", new Texture("gui/Button", TextureMinFilter.Nearest));
            textures.Add("crosshair", new Texture("gui/Crosshair", TextureMinFilter.Nearest));
            textures.Add("hotbar", new Texture("gui/hotbar", TextureMinFilter.Nearest));
            textures.Add("rainbow", new Texture("gui/Rainbow", TextureMinFilter.Nearest));

            models.Add("cube", new TexturedModel(OBJLoader.loadOBJ("Cube"), textures["blocks"]));
            models.Add("cursor", new TexturedModel(OBJLoader.loadOBJ("Cursor"), textures["rainbow"]));
        }
    }
}
