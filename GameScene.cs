using KonradCraft.gui;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using KonradCraft.scenes;
using KonradCraft.world;
using KonradCraft.entities;
using KonradCraft.shaders;

namespace KonradCraft
{
    internal class GameScene : Scene
    {
        Skybox skybox;
        private MouseState mouse;
        private Camera cam;


        GUIText fpsText;

        Chunk[,] chunks = new Chunk[8, 8];

        public Inventory inventory;

        public Chunk? getChunk(int x, int y, int z)
        {
            /*
            chunks[4, 4].setBlock(x, y, z, block);
            chunks[4, 4].updateChunk();
            */
            int cX = (int)MathF.Floor(x / 16f);
            int cY = (int)MathF.Floor(z / 16f);
            foreach(Chunk c in chunks)
            {
                if (c.xPos == cX && c.zPos == cY) return c;
            }
            return null;
        }

        //List<Chunk> chunks = new List<Chunk>();

        public GameScene(MouseState mouse, EntityManager entityManager, GUIManager guiManager, SceneManager sceneManager) : base(entityManager, guiManager, sceneManager)
        {
            this.mouse = mouse;
            cam = Toolbox.camera;
        }
        TerrainGenerator generator;
        Player player;
        public Hotbar hotbar;
        public override void Start()
        {
            Toolbox.camera.position = new Vector3(0f, 60f, 0f);
            skybox = new Skybox("BlueSky", Toolbox.camera, OBJLoader.loadOBJ("Cube"));
            generator = new TerrainGenerator();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chunks[i, j] = new Chunk(i - 4, j - 4, generator);
                }
            }

            fpsText = new GUIText(new Vector2(-3.3f, 3.3f), 0.2f, Fonts.FONT_LARGE, "FPS");
            guiManager.Add(fpsText);
            guiManager.Add(new GUIObject(Vector2.Zero, Vector2.One* 0.1f, Toolbox.textures["crosshair"]));
            Entity cursor = new Entity(Vector3.Zero, Toolbox.models["cursor"], (EntityShader)Toolbox.shaders["cursor"]);

            cursor.scale = Vector3.One*0.55f;
            entityManager.addEntity(cursor);
            Items.loadItems();
            player = new Player(new Vector3(0, 60, 0), cam, generator, mouse, this, cursor);
            entityManager.addEntity(player);
            inventory = new Inventory();
            inventory.items[0] = new ItemStack(Items.items[0], 64);
            inventory.items[1] = new ItemStack(Items.items[1], 64);
            hotbar = new Hotbar(mouse, player, inventory);
            guiManager.Add(hotbar);
        }
        public override void Update()
        {
            Vector3 pos = player.position;
            foreach (Chunk c in chunks)
            {
                c.update();
            }
            base.Update();
            Vector2i oldChunkPos = new Vector2i((int)MathF.Floor(pos.X / 16f), (int)MathF.Floor(pos.Z / 16f));
            //updateCamera();
            pos = player.position;
            Vector2i newChunkPos = new Vector2i((int)MathF.Floor(pos.X / 16f), (int)MathF.Floor(pos.Z / 16f));
            if (newChunkPos.X < oldChunkPos.X)
            {
                Chunk[,] temp = new Chunk[8, 8];

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        temp[(x + 1) % 8, y] = chunks[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        chunks[x, y] = temp[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    Chunk c = chunks[0, x];
                    c.moveChunk(chunks[1, x].xPos - 1, c.zPos);
                }
            }
            else if (newChunkPos.X > oldChunkPos.X)
            {
                Chunk[,] temp = new Chunk[8, 8];

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int newX = x;
                        newX -= 1;
                        if (newX < 0) newX = 7;
                        temp[newX % 8, y] = chunks[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        chunks[x, y] = temp[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    Chunk c = chunks[7, x];
                    c.moveChunk(chunks[6, x].xPos + 1, c.zPos);
                }
            }
            if (newChunkPos.Y < oldChunkPos.Y)
            {
                Chunk[,] temp = new Chunk[8, 8];

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        temp[x, (y + 1) % 8] = chunks[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        chunks[x, y] = temp[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    Chunk c = chunks[x, 0];
                    c.moveChunk(c.xPos, chunks[x, 1].zPos - 1);
                }
            }
            else if (newChunkPos.Y > oldChunkPos.Y)
            {
                Chunk[,] temp = new Chunk[8, 8];

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int newY = y;
                        newY -= 1;
                        if (newY < 0) newY = 7;
                        temp[x, newY] = chunks[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        chunks[x, y] = temp[x, y];
                    }
                }
                for (int x = 0; x < 8; x++)
                {
                    Chunk c = chunks[x, 7];
                    c.moveChunk(c.xPos, chunks[x, 6].zPos + 1);
                }
            }

            if (Keyboard.getKeyDown(Keys.P)) useLineMode = !useLineMode;

            /*if (Keyboard.getKeyDown(Keys.Space))
            {
                if (pos.Y < 127)
                {
                    Vector3i posInChunk = new Vector3i((int)MathF.Floor(cam.position.X) - chunks[4, 4].xPos * 16, (int)MathF.Floor(cam.position.Y), (int)MathF.Floor(cam.position.Z) - chunks[4, 4].zPos * 16);
                    chunks[4, 4].setBlock(posInChunk.X, posInChunk.Y, posInChunk.Z, 7);
                    chunks[4, 4].updateChunk();
                    generator.setBlock((int)MathF.Floor(cam.position.X), (int)MathF.Floor(cam.position.Y), (int)MathF.Floor(cam.position.Z), 7);
                }
            }*/
        }
        bool useLineMode = false;
        double lastFrame = 0f;
        int averageAmount = 0;
        double averageCounter = 0;
        public override void Render()
        {
            double timeSinceLastFrame = Time.timeDouble - lastFrame;
            double fps = 1.0 / timeSinceLastFrame;
            averageCounter += fps;
            averageAmount++;
            if (averageAmount > 500)
            {
                fpsText.Text = "FPS " + Math.Floor(averageCounter / 500.0);
                averageCounter = 0;
                averageAmount = 0;
            }
            Clear(ClearBufferMask.DepthBufferBit);
            if (useLineMode)
                PolygonMode(MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
            foreach (Chunk chunk in chunks)
            {
                //if(Vector2.Distance(new Vector2(chunk.xPos*16f,chunk.zPos*16f), cam.position.Xz) < 60f)
                chunk.Render();
            }
            if(useLineMode)
            PolygonMode(MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Fill);

            Disable(EnableCap.CullFace);
            skybox.Render();
            Enable(EnableCap.CullFace);
            base.Render();
            lastFrame = Time.timeDouble;
        }
        public override void OnClose()
        {

        }
    }
}
