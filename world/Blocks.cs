using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonradCraft.world
{
    class BlockTexture
    {
        public float u; public float v;
        public BlockTexture(float u, float v)
        {
            this.u = u;
            this.v = v;
        }
        public BlockTexture(int x, int y)
        {
            u = x / 8f;
            v = y / 8f;
        }
        public BlockTexture(int i)
        {
            int x = i % 8;
            int y = i / 8;

            u = x / 8f;
            v = y / 8f;
        }
    }
    class Block
    {
        public BlockTexture up;
        public BlockTexture down;
        public BlockTexture left;
        public BlockTexture right;
        public BlockTexture front;
        public BlockTexture back;

        public bool solid = true;
        
        public Block(bool solid, int all)
        {
            up = down = left = right = front = back = new BlockTexture(all);
            this.solid = solid;
        }
        public Block(bool solid, int ring, int y)
        {
            up = down = new BlockTexture(y);
            left = right = front = back = new BlockTexture(ring);
            this.solid = solid;
        }
        public Block(bool solid, int ring, int top, int bottom)
        {
            up = new BlockTexture(top);
            down = new BlockTexture(bottom);
            left = right = front = back = new BlockTexture(ring);
            this.solid = solid;
        }
    }
    internal class Blocks
    {
        public static Block DIRT = new(true,0);
        public static Block GRASS = new(true, 1, 2, 0);
        public static Block STONE = new(true, 3);
        public static Block SAND = new(true, 4);
        public static Block LOG = new(true, 5,6);
        public static Block LEAVES = new(true, 7);
        public static Block PLANKS = new(true, 8);
        public static Block PINK_TULIP = new(false,9);
        public static Block LILAC_FLOWER = new(false,10);

        public static List<Block> BLOCKS = [DIRT,GRASS,STONE,SAND,LOG,LEAVES,PLANKS,PINK_TULIP,LILAC_FLOWER];
    }
}
