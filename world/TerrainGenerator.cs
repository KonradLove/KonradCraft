using OpenTK.Mathematics;
using SimplexNoise;

namespace EminaCraft.world
{
    internal class TerrainGenerator
    {
        Dictionary<(int x, int y, int z), byte> modifiedBlocks = new();

        public (int, int, int, int, int) posToChunkPos(int x, int y, int z)
        {
            Vector2i newChunkPos = new Vector2i((int)MathF.Floor(x / 16f), (int)MathF.Floor(z / 16f));
            Vector3i posInChunk = new Vector3i(x % 16, y, z % 16);
            return (newChunkPos.X, newChunkPos.Y, posInChunk.X, posInChunk.Y, posInChunk.Z);
        }
        public (int, int, int) chunkPosToWorld(int x, int y, int z, int cx, int cz)
        {
            return (x + cx * 16, y, z + cz * 16);
        }

        public void setBlock(int x, int y, int z, byte block)
        {
            modifiedBlocks[(x, y, z)]=block;
            //(int cx, int cy, int _x, int _y, int _z) = posToChunkPos(x, y, z);
            //setBlock(_x,_y,_z,cx,cy, block);
        }
        public void setBlock(int x, int y, int z, int cx, int cy, byte block)
        {
            (int _x, int _y, int _z) = chunkPosToWorld(x, y, z, cx, cy);
            if (modifiedBlocks.ContainsKey((_x, _y, _z)))
            {
                //if (modifiedBlocks[(cx, cy)].ContainsKey((x, y, z))) modifiedBlocks[(cx, cy)].Remove((x, y, z));
                modifiedBlocks[(_x, _y, _z)]=block;
            }

        }

        public TerrainGenerator()
        {
        }

        bool getAir(int x, int y, int z)
        {
            return Noise.CalcPixel3D(x, y, z, 0.03f) > 220f;
        }
        public int getBlock(int x, int y, int z)
        {
            if(modifiedBlocks.ContainsKey((x, y, z)))
                return modifiedBlocks[(x, y, z)];
            
            
            if (getAir(x, y, z)) return 0;
            //float grassLevel = (1.2f + MathF.Sin((x + z) / 20f)) * 20;
            float grassLevel = 30f + (noise(x, z, 0.01f) * 20f);
            grassLevel += noise(x, z, 0.003f) * 40f * noise(x, z, 0.03f);
            if (y < grassLevel)
            {
                if (y + 1 >= grassLevel)
                    return 2;

                if (grassLevel - y > 4) return 3;
                return 1;

            }
            else if (y - 1 < grassLevel && !getAir(x, y - 1, z))
            {
                if (noise(x, z, 0.1f) > 0.9)
                {
                    return 8;
                }
                else if (noise(x + 16832, z + 56742, 0.1f) > 0.9)
                {
                    return 9;
                }
            }
            return 0;
        }

        private float noise(int x, int z, float scale)
        {
            return MathF.Min(1f, Noise.CalcPixel2D(x, z, scale) / 255f);
        }
        public struct RaycastResult
        {
            public bool hit;
            public Vector3i result;
            public int block;
            public Vector3i face;

            public RaycastResult(bool hit, Vector3i result, int block, Vector3i face)
            {
                this.hit = hit;
                this.result = result;
                this.block = block;
                this.face = face;
            }
        }
        public RaycastResult Raycast(Vector3 origin, Vector3 direction, int maxDistance)
        {
            Vector3i blockPos = new Vector3i((int)Math.Floor(origin.X), (int)Math.Floor(origin.Y), (int)Math.Floor(origin.Z));

            Vector3 step = new Vector3(
                direction.X > 0 ? 1 : -1,
                direction.Y > 0 ? 1 : -1,
                direction.Z > 0 ? 1 : -1
            );

            Vector3 tMax = new Vector3(
                (blockPos.X + (direction.X > 0 ? 1 : 0) - origin.X) / direction.X,
                (blockPos.Y + (direction.Y > 0 ? 1 : 0) - origin.Y) / direction.Y,
                (blockPos.Z + (direction.Z > 0 ? 1 : 0) - origin.Z) / direction.Z
            );

            Vector3 tDelta = new Vector3(
                Math.Abs(1 / direction.X),
                Math.Abs(1 / direction.Y),
                Math.Abs(1 / direction.Z)
            );
            int face = 0;
            for (int i = 0; i < maxDistance; i++)
            {
                // Check if block is solid
                int block = getBlock(blockPos.X, blockPos.Y, blockPos.Z);
                if (block != 0)
                {
                    Vector3i faceDir = Vector3i.Zero;
                    if (face == 1 && step.Y < 0) faceDir = Vector3i.UnitY;
                    if (face == 1 && step.Y > 0) faceDir = -Vector3i.UnitY;

                    if (face == 0 && step.X < 0) faceDir = Vector3i.UnitX;
                    if (face == 0 && step.X > 0) faceDir = -Vector3i.UnitX;

                    if (face == 2 && step.Z < 0) faceDir = Vector3i.UnitZ;
                    if (face == 2 && step.Z > 0) faceDir = -Vector3i.UnitZ;



                    return new RaycastResult(true, blockPos, block, faceDir); // Found a solid block
                }

                // Step to the next voxel
                if (tMax.X < tMax.Y && tMax.X < tMax.Z)
                {
                    blockPos.X += (int)step.X;
                    tMax.X += tDelta.X;
                    face = 0;


                }
                else if (tMax.Y < tMax.Z)
                {
                    blockPos.Y += (int)step.Y;
                    tMax.Y += tDelta.Y;
                    face = 1;
                }
                else
                {
                    blockPos.Z += (int)step.Z;
                    tMax.Z += tDelta.Z;
                    face = 2;
                }
            }

            return new RaycastResult(false, Vector3i.Zero, 0, Vector3i.Zero); // No solid block found within range
        }
    }
}
