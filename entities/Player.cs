using EminaCraft.world;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EminaCraft.world.TerrainGenerator;

namespace EminaCraft.entities
{
    internal class Player : Entity
    {
        public bool sprinting = false;
        private Camera cam;
        private TerrainGenerator generator;
        InputAxis MOVE = new InputAxis(Keys.D, Keys.A, Keys.S, Keys.W);
        InputSystem input = new InputSystem();
        MouseState mouse;
        public int health = 16;
        private GameScene game;
        private Entity cursor;
        public Player(Vector3 position, Camera cam, TerrainGenerator generator, MouseState mouse, GameScene game, Entity cursor) : base(position, new EmptyRenderable())
        {
            this.cam = cam;
            this.generator = generator;
            input.axes.Add(MOVE);
            this.mouse = mouse;
            this.game = game;
            this.cursor = cursor;
        }
        float velY = 0f;
        public override void Tick()
        {
            
            input.updateInputs();
            cam.position = new Vector3(position.X, position.Y+1.75f, position.Z+0.01f);
            if (!checkCollision(new Vector3(position.X,position.Y+velY,position.Z)))
            {
                //velY -= Time.deltaTime * 0.03f;
                position = tryMove(position, new Vector3(0, velY, 0));
            }
            else
            {
                velY = 0f;
                
            }
            bool grounded = checkCollision(position - Vector3.UnitY*0.0001f);
            if ( Keyboard.getKey(Keys.Space))
            {
                velY = Time.deltaTime * 3f;
            }else if (Keyboard.getKey(Keys.LeftShift))
            {
                velY = -Time.deltaTime * 3f;
            }
            else
            {
                velY = 0;
            }
                /*{
                    velY = 0.0125f;
                    position = tryMove(position, new Vector3(0, velY, 0));
                }
                if (!grounded)
                {
                    if(checkCollision(position + new Vector3(0, 1.801f, 0))){
                        velY = -0.00135f;
                    }
                }*/
                Vector2 mv = -MOVE.getVec2();
            if(mv.Length > 1)
            {
                mv = mv.Normalized();
            }
            Vector2 moveVec = new Vector2();

            moveVec.X = MathF.Cos(cam.rotation.Y+MathF.PI/2f) * mv.Y * (sprinting ? 1.2f : 1f);
            moveVec.Y = MathF.Sin(cam.rotation.Y + MathF.PI / 2f) * mv.Y * (sprinting ? 1.2f : 1f);

            moveVec.X += MathF.Cos(cam.rotation.Y) * mv.X;
            moveVec.Y += MathF.Sin(cam.rotation.Y) * mv.X;

            position = tryMove(position, new Vector3(moveVec.X * Time.deltaTime * 4f, 0, 0));
            position = tryMove(position, new Vector3(0,0, moveVec.Y * Time.deltaTime * 4f));
            cam.rotation.Y += mouse.Delta.X * 0.001f;
            cam.rotation.X += mouse.Delta.Y * 0.001f;
            cam.rotation.X = cam.rotation.X < -MathF.PI/2f ? -MathF.PI / 2f : (cam.rotation.X > MathF.PI/2f ? MathF.PI / 2f : cam.rotation.X);
            if (Keyboard.getKeyDown(Keys.LeftControl)){
                sprinting = !sprinting;
                cam.fov = sprinting ? 100 : 90;
                health--;
            }
            //RaycastResult result = generator.Raycast(cam.position, new Vector3(0,0,1), 4);
            Vector3 dir = GetDirectionFromRotation(cam.rotation.Y-MathF.PI/2f, cam.rotation.X);

            RaycastResult result = generator.Raycast(cam.position, dir, 6);
            if (result.hit)
            {
                cursor.visible = true;
                cursor.position = result.result + Vector3.One*0.5f;
            }
            else
            {
                cursor.visible = false;
            }
            ItemStack? item = game.inventory.items[game.inventory.selectedItem];
            if (mouse.IsButtonPressed(MouseButton.Right) && result.hit 
                && item!=null
                && item.item.block!=-1)
            {
                Vector3i newBlockPos = result.result + result.face;
                if (checkPlayerNotHittingBlock(newBlockPos))
                {
                    item.amount--;
                    if(item.amount <= 0)
                    {
                        game.inventory.items[game.inventory.selectedItem] = null;
                    }
                    generator.setBlock(newBlockPos.X, newBlockPos.Y, newBlockPos.Z, (byte)(item.item.block));
                    Chunk? c = game.getChunk(newBlockPos.X, newBlockPos.Y, newBlockPos.Z);
                    if (c != null)
                    {
                        Vector3i newPos = new Vector3i(
                                ModWrap(newBlockPos.X, 16),
                                newBlockPos.Y,
                                ModWrap(newBlockPos.Z, 16)
                            );

                        c.setBlock(newPos.X, newPos.Y, newPos.Z, (byte)(item.item.block));
                        c.updateChunk();
                    }
                }
                
            }

            if (mouse.IsButtonPressed(MouseButton.Left) && result.hit)
            {

                game.inventory.addItemByBlock(generator.getBlock(result.result.X, result.result.Y, result.result.Z),1);
                generator.setBlock(result.result.X, result.result.Y, result.result.Z, 0);
                Chunk? c = game.getChunk(result.result.X, result.result.Y, result.result.Z);
                if (c != null)
                {
                    Vector3i newPos = new Vector3i(
                            ModWrap(result.result.X,16),
                            result.result.Y,
                            ModWrap(result.result.Z,16)
                        );
                    c.setBlock(newPos.X, newPos.Y, newPos.Z, 0);
                    c.updateChunk();
                    if (newPos.X == 0) game.getChunk(result.result.X - 1, result.result.Y, result.result.Z).updateChunk();
                    if (newPos.X == 15) game.getChunk(result.result.X + 1, result.result.Y, result.result.Z).updateChunk();
                    if (newPos.Z == 0) game.getChunk(result.result.X, result.result.Y, result.result.Z-1).updateChunk();
                    if (newPos.Z == 15) game.getChunk(result.result.X, result.result.Y, result.result.Z+1).updateChunk();
                }
            }
            
        }
        int ModWrap(int a, int b)
        {
            return ((a % b) + b) % b;
        }
        Vector3 GetDirectionFromRotation(float yaw, float pitch)
        {

            float x = (float)(Math.Cos(pitch) * Math.Cos(yaw));
            float y = -(float)(Math.Sin(pitch));
            float z = (float)(Math.Cos(pitch) * Math.Sin(yaw));

            return new Vector3(x, y, z).Normalized();
        }
        private Vector3 tryMove(Vector3 pos, Vector3 dir)
        {
            Vector3i intPos = new Vector3i((int)MathF.Floor(position.X), (int)MathF.Floor(position.Y), (int)MathF.Floor(position.Z));
            Vector3i newIntPos = new Vector3i((int)MathF.Floor(position.X+dir.X), (int)MathF.Floor(position.Y + dir.Y), (int)MathF.Floor(position.Z+dir.Z));

            float dist = 0.1f;
            if (checkBoxEmpty(pos + dir, dist) && checkBoxEmpty(pos + dir + new Vector3(0,0.9f,0),dist) && checkBoxEmpty(pos + dir + new Vector3(0, 1.8f, 0),dist)) return pos+dir;
            return pos;
        }
        bool checkBoxEmpty(Vector3 pos, float dist)
        {
            return !checkCollision(pos + new Vector3(dist,0,dist)) && !checkCollision(pos + new Vector3(-dist, 0, dist)) &&
                !checkCollision(pos + new Vector3(-dist, 0, -dist)) && !checkCollision(pos + new Vector3(dist, 0, -dist));
        }
        bool checkBoxEmpty(Vector3 pos, Vector3i block, float dist)
        {
            return blockPos(pos + new Vector3(dist, 0, dist))!=block && blockPos(pos + new Vector3(-dist, 0, dist)) != block &&
                blockPos(pos + new Vector3(-dist, 0, -dist)) != block && blockPos(pos + new Vector3(dist, 0, -dist)) != block;
        }
        private bool checkCollision(Vector3 position)
        {
            Vector3i intPos = new Vector3i((int)MathF.Floor(position.X), (int)MathF.Floor(position.Y), (int)MathF.Floor(position.Z));
            int block = generator.getBlock(intPos.X, intPos.Y, intPos.Z);
            if (block != 0)
            {
                return Blocks.BLOCKS[block-1].solid;
            }
            return false;
        }
        private Vector3i blockPos(Vector3 position)
        {
            Vector3i intPos = new Vector3i((int)MathF.Floor(position.X), (int)MathF.Floor(position.Y), (int)MathF.Floor(position.Z));
            return intPos;
        }
        bool checkPlayerNotHittingBlock(Vector3i blockPos)
        {
            float dist = 0.1f;
            return (checkBoxEmpty(position, blockPos, dist) && checkBoxEmpty(position + new Vector3(0, 0.9f, 0), blockPos, dist) &&
                checkBoxEmpty(position + new Vector3(0, 1.8f, 0), blockPos, dist));
            
        }


    }
}
