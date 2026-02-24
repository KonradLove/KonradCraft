using KonradCraft.world;
using OpenTK.Mathematics;

namespace KonradCraft.shaders
{
    internal class ChunkShader : ShaderProgram
    {
        protected UniformMatrix4 projectionU;
        protected UniformMatrix4 viewU;
        protected UniformMatrix4 transformationU;

        private UniformVec3 camPosU;

        private Camera cam;
        public ChunkShader(Camera cam) : base("chunk")
        {
            this.cam = cam;
            transformationU = new UniformMatrix4("model", shaderHandle);
            projectionU = new UniformMatrix4("projection", shaderHandle);
            viewU = new UniformMatrix4("view", shaderHandle);
            camPosU = new UniformVec3("camPos", shaderHandle);
        }
        public Chunk? chunk;
        public override void uploadUniforms()
        {
            if (chunk == null) return;
            viewU.upload(cam.viewMatrix);
            projectionU.upload(cam.generateProjectionMatrix());
            transformationU.upload(
                Matrix4.CreateTranslation(new Vector3(chunk.xPos*16f, 0f, chunk.zPos*16f))
            );
            camPosU.upload(cam.position);
        }
    }
}
