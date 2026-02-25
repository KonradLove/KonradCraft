using OpenTK.Mathematics;

namespace EminaCraft.shaders
{
    internal class DefaultShader : EntityShader
    {
        protected UniformFloat blendU;
        public DefaultShader(Camera cam) : base(cam,"default")
        {
            blendU = new UniformFloat("blend", shaderHandle);
        }
        public override void uploadUniforms()
        {
            base.uploadUniforms();
            blendU.upload(currentEntity.opacity);
        }
    }
}
