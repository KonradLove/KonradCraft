using EminaCraft.shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EminaCraft.world
{
    internal class Chunk
    {
        SimpleModel? model;
        private ChunkShader shader;
        private TerrainGenerator generator;
        private Texture blocksTexture;
        public int xPos, zPos;


        int[,,] data = new int[16, 128, 16];

        public Chunk(int xPos, int zPos, TerrainGenerator generator)
        {
            this.xPos = xPos;
            this.generator = generator;
            this.zPos = zPos;
            shader = (ChunkShader)Toolbox.shaders["chunk"];
            blocksTexture = Toolbox.textures["blocks"];

            chunkLoadMain().Wait();

            //model = Loader.loadToVAO(vertices.ToArray(), indices.ToArray());
        }
        List<float> vertices = new List<float>();
        List<int> indices = new List<int>();
        public async Task loadChunk()
        {
            loadingChunk = true;
            canDraw = false;
            vertices.Clear();
            indices.Clear();
            List<Vector3i> treePositions = new List<Vector3i>();
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        data[x, y, z] = generator.getBlock(x + xPos * 16, y, z + zPos * 16);
                        if (data[x, y, z] == 2)
                        {
                            float randNum = rand(x, z);
                            if (randNum > 0.6 && randNum < 0.601)
                                treePositions.Add(new Vector3i(x, y + 1, z));
                        }
                    }
                }
            }
            foreach (Vector3i pos in treePositions)
            {
                for (int x = pos.X - 2; x < pos.X + 3; x++)
                {
                    for (int y = pos.Y + 3; y < pos.Y + 5; y++)
                    {
                        for (int z = pos.Z - 2; z < pos.Z + 3; z++)
                        {
                            generator.setBlock(x, y, z, xPos, zPos, 6);
                            setBlock(x, y, z, 6);
                        }
                    }
                }
                for (int x = pos.X - 1; x < pos.X + 2; x++)
                {
                    for (int z = pos.Z - 1; z < pos.Z + 2; z++)
                    {
                        generator.setBlock(x, pos.Y + 5, z, xPos, zPos, 6);
                        setBlock(x, pos.Y + 5, z, 6);
                    }
                }

                setBlock(pos.X - 1, pos.Y + 6, pos.Z, 6);
                setBlock(pos.X + 1, pos.Y + 6, pos.Z, 6);
                setBlock(pos.X, pos.Y + 6, pos.Z + 1, 6);
                setBlock(pos.X, pos.Y + 6, pos.Z - 1, 6);
                setBlock(pos.X, pos.Y + 6, pos.Z, 6);

                generator.setBlock(pos.X - 1, pos.Y + 6, pos.Z, xPos, zPos, 6);
                generator.setBlock(pos.X + 1, pos.Y + 6, pos.Z, xPos, zPos, 6);
                generator.setBlock(pos.X, pos.Y + 6, pos.Z + 1, xPos, zPos, 6);
                generator.setBlock(pos.X, pos.Y + 6, pos.Z - 1, xPos, zPos, 6);
                generator.setBlock(pos.X, pos.Y + 6, pos.Z, xPos, zPos, 6);
                for (int i = pos.Y; i < pos.Y + 6; i++)
                {
                    setBlock(pos.X, i, pos.Z, 5);
                    generator.setBlock(pos.X, i, pos.Z, xPos, zPos, 5);
                }
            }
            loadModel();
            loadingChunk = false;
        }
        private void loadModel()
        {
            int cVert = 0;
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        //gen mesh
                        int block = data[x, y, z];
                        if (block > 0)
                        {
                            if(block==8 || block == 9) //X Block
                            {
                                BlockTexture b = Blocks.BLOCKS[block - 1].front;

                                addVertex(vertices, x, y, z, 0, 0, -1, 0, 1, b);
                                addVertex(vertices, x + 1, y, z+1, 0, 0, -1, 1, 1, b);
                                addVertex(vertices, x + 1, y + 1, z+1, 0, 0, -1, 1, 0, b);
                                addVertex(vertices, x, y + 1, z, 0, 0, -1, 0, 0, b);

                                indices.Add(cVert + 2); indices.Add(cVert + 1); indices.Add(cVert);
                                indices.Add(cVert); indices.Add(cVert + 3); indices.Add(cVert + 2);

                                indices.Add(cVert); indices.Add(cVert + 1); indices.Add(cVert+2);
                                indices.Add(cVert+2); indices.Add(cVert + 3); indices.Add(cVert);

                                cVert += 4;

                                addVertex(vertices, x, y, z+1, 0, 0, -1, 0, 1, b);
                                addVertex(vertices, x + 1, y, z, 0, 0, -1, 1, 1, b);
                                addVertex(vertices, x + 1, y + 1, z, 0, 0, -1, 1, 0, b);
                                addVertex(vertices, x, y + 1, z+1, 0, 0, -1, 0, 0, b);

                                indices.Add(cVert + 2); indices.Add(cVert + 1); indices.Add(cVert);
                                indices.Add(cVert); indices.Add(cVert + 3); indices.Add(cVert + 2);

                                indices.Add(cVert); indices.Add(cVert + 1); indices.Add(cVert + 2);
                                indices.Add(cVert + 2); indices.Add(cVert + 3); indices.Add(cVert);

                                cVert += 4;
                            }
                            else
                            {
                                // Front face (z-negative)
                                if (isTransparent(getBlock(x, y, z - 1)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].front;
                                    addVertex(vertices, x, y, z, 0, 0, -1, 0, 1, b);
                                    addVertex(vertices, x + 1, y, z, 0, 0, -1, 1, 1, b);
                                    addVertex(vertices, x + 1, y + 1, z, 0, 0, -1, 1, 0, b);
                                    addVertex(vertices, x, y + 1, z, 0, 0, -1, 0, 0, b);
                                    indices.Add(cVert + 2); indices.Add(cVert + 1); indices.Add(cVert);
                                    indices.Add(cVert); indices.Add(cVert + 3); indices.Add(cVert + 2);
                                    cVert += 4;
                                }


                                // Back face (z-positive)
                                if (isTransparent(getBlock(x, y, z + 1)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].back;
                                    addVertex(vertices, x, y, z + 1, 0, 0, 1, 0, 1, b);
                                    addVertex(vertices, x + 1, y, z + 1, 0, 0, 1, 1, 1, b);
                                    addVertex(vertices, x + 1, y + 1, z + 1, 0, 0, 1, 1, 0, b);
                                    addVertex(vertices, x, y + 1, z + 1, 0, 0, 1, 0, 0, b);
                                    indices.Add(cVert); indices.Add(cVert + 1); indices.Add(cVert + 2);
                                    indices.Add(cVert + 2); indices.Add(cVert + 3); indices.Add(cVert);
                                    cVert += 4;
                                }

                                // Left face (x-negative)
                                if (isTransparent(getBlock(x - 1, y, z)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].left;
                                    addVertex(vertices, x, y, z, -1, 0, 0, 0, 1, b);
                                    addVertex(vertices, x, y, z + 1, -1, 0, 0, 1, 1, b);
                                    addVertex(vertices, x, y + 1, z + 1, -1, 0, 0, 1, 0, b);
                                    addVertex(vertices, x, y + 1, z, -1, 0, 0, 0, 0, b);
                                    indices.Add(cVert); indices.Add(cVert + 1); indices.Add(cVert + 2);
                                    indices.Add(cVert + 2); indices.Add(cVert + 3); indices.Add(cVert);
                                    cVert += 4;
                                }

                                // Right face (x-positive)
                                if (isTransparent(getBlock(x + 1, y, z)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].right;
                                    addVertex(vertices, x + 1, y, z, 1, 0, 0, 0, 1, b);
                                    addVertex(vertices, x + 1, y, z + 1, 1, 0, 0, 1, 1, b);
                                    addVertex(vertices, x + 1, y + 1, z + 1, 1, 0, 0, 1, 0, b);
                                    addVertex(vertices, x + 1, y + 1, z, 1, 0, 0, 0, 0, b);
                                    indices.Add(cVert + 2); indices.Add(cVert + 1); indices.Add(cVert);
                                    indices.Add(cVert); indices.Add(cVert + 3); indices.Add(cVert + 2);
                                    cVert += 4;
                                }

                                // Top face (Y-positive)
                                if (isTransparent(getBlock(x, y + 1, z)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].up;
                                    addVertex(vertices, x, y + 1, z, 0, 1, 0, 0, 0, b);
                                    addVertex(vertices, x + 1, y + 1, z, 0, 1, 0, 1, 0, b);
                                    addVertex(vertices, x + 1, y + 1, z + 1, 0, 1, 0, 1, 1, b);
                                    addVertex(vertices, x, y + 1, z + 1, 0, 1, 0, 0, 1, b);
                                    indices.Add(cVert + 2); indices.Add(cVert + 1); indices.Add(cVert);
                                    indices.Add(cVert); indices.Add(cVert + 3); indices.Add(cVert + 2);
                                    cVert += 4;
                                }

                                // Bottom face (Y-negative)
                                if (isTransparent(getBlock(x, y - 1, z)))
                                {
                                    BlockTexture b = Blocks.BLOCKS[block - 1].down;
                                    addVertex(vertices, x, y, z, 0, -1, 0, 0, 0, b);
                                    addVertex(vertices, x + 1, y, z, 0, -1, 0, 1, 0, b);
                                    addVertex(vertices, x + 1, y, z + 1, 0, -1, 0, 1, 1, b);
                                    addVertex(vertices, x, y, z + 1, 0, -1, 0, 0, 1, b);
                                    indices.Add(cVert); indices.Add(cVert + 1); indices.Add(cVert + 2);
                                    indices.Add(cVert + 2); indices.Add(cVert + 3); indices.Add(cVert);
                                    cVert += 4;
                                }
                            }
                            
                        }
                    }
                }
            }
        }
        async Task chunkLoadMain()
        {
            await Task.Run(async () =>
            {
                await loadChunk();
            });
            
        }
        bool isTransparent(int block)
        {
            return (block==0 || block==8 || block==9) ? true : false;
        }
        private bool loadingChunk = false;
        private bool canDraw = true;
        public void moveChunk(int x, int z)
        {
            canDraw = false;
            xPos = x;
            zPos = z;
            chunkLoadMain().Wait();
        }
        public void updateChunk()
        {
            vertices.Clear();
            indices.Clear();
            loadModel();
            canDraw = false;
        }
        public void update()
        {

        }
        float rand(int x, int y)
        {
            int hash = x * 73856093 ^ y * 19349663;
            hash = hash * hash * y*y*2132187;

            return (hash >= 0 ? hash : -hash) % 255 / 255f;
        }
        public int getBlock(int x, int y, int z)
        {
            if (x < 0 || x >= 16 || y < 0 || y >= 128 || z < 0 || z >= 16)
            {
                return generator.getBlock(x + xPos*16, y, z + zPos * 16);
            }
            return data[x, y, z];
        }
        public void setBlock(int x, int y, int z, int type)
        {
            if (x < 0 || x >= 16 || y < 0 || y >= 128 || z < 0 || z >= 16) return;
            data[x, y, z] = type;
        }
        void addVertex(List<float> vertices, float x, float y, float z, float nx, float ny, float nz, float u, float v, BlockTexture texture)
        {
            vertices.Add(x);
            vertices.Add(y);
            vertices.Add(z);
            vertices.Add(nx);
            vertices.Add(ny);
            vertices.Add(nz);

            vertices.Add(u/8f + texture.u);
            vertices.Add(v/8f + texture.v);
        }

        public void Render()
        {
            if(loadingChunk == false && canDraw == false)
            {
                loadingChunk = false;
                canDraw = true;
                if (model == null)
                    model = Loader.loadToVAO(vertices.ToArray(), indices.ToArray());
                else Loader.loadToExistingVAO(vertices.ToArray(), indices.ToArray(), model);
            }
            
            if (model == null || loadingChunk) return;
            shader.bind();
            shader.chunk = this;
            shader.uploadUniforms();
            blocksTexture.bind(TextureUnit.Texture0);
            model.Bind();
            model.Render();
            blocksTexture.unbind(TextureUnit.Texture0);
            model.Unbind();
            shader.unbind();
        }
        
    }
}
