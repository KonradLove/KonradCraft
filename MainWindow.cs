using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using EminaCraft.gui;
using EminaCraft.shaders;
using EminaCraft.scenes;

namespace EminaCraft
{
    internal class MainWindow : GameWindow
    {
        SceneManager sceneManager;
        EntityManager entities = new EntityManager();
        GUIManager gui;
        public static MainWindow instance;
        public MainWindow() : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = "CS Project",                   //Title of the window
                ClientSize = new Vector2i(1920, 1080),    //Window size
                WindowBorder = WindowBorder.Hidden,      //Window cannot be resized
                StartVisible = false,                   //Window only set visible after initialised to avoid funny behaviour
                StartFocused = true,                    //Focuses the window upon creation
                Vsync = VSyncMode.Off,                   //Frame rate matched to monitors frame rate
                API = ContextAPI.OpenGL,                //Using OpenGL
                Profile = ContextProfile.Core,          //Core OpenGL
                APIVersion = new Version(3, 3)          //OpenGL version 3.3
            })
        {
            CenterWindow();
            instance = this;
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Keyboard.keyDown(e.Key);
            base.OnKeyDown(e);
            
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Keyboard.keyUp(e.Key);
            base.OnKeyUp(e);
        }
        
        protected override void OnLoad()
        {
            Time.init();
            IsVisible = true;
            CursorState = CursorState.Grabbed;
            ClearColor(OpenTK.Mathematics.Color4.DarkCyan); //i like this colour
            Enable(EnableCap.DepthTest);        //For later on when 3d stuff is happening
            Enable(EnableCap.CullFace);
            CullFace(CullFaceMode.Back);
            Viewport(0, 0, ClientSize.X, ClientSize.Y);
            Toolbox.camera = new Camera(new Vector3(0f, 3f, 2f), Vector3.Zero);
            Toolbox.initToolbox();
            FrontFace(FrontFaceDirection.Ccw);
            DefaultShader shader = new DefaultShader(Toolbox.camera);
            Toolbox.shaders.Add("default", shader);
            Toolbox.shaders.Add("chunk", new ChunkShader(Toolbox.camera));
            Toolbox.shaders.Add("cursor", new CursorShader(Toolbox.camera));

            gui = new GUIManager(1080f / 1920f);
            sceneManager = new SceneManager(entities, gui);

            Fonts.initFonts();

            sceneManager.LoadScene(new GameScene(MouseState, entities, gui, sceneManager));
            
            base.OnLoad();
        }
        //acab even the memory police
        protected override void OnUnload()
        {
            sceneManager.Unload();
            base.OnUnload();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            sceneManager.Update();
            Time.update();
            Toolbox.camera.update();
            base.OnUpdateFrame(args);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            sceneManager.Render();
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        

    }
}
