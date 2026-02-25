using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using static OpenTK.Graphics.OpenGL.GL;

namespace EminaCraft
{
    internal abstract class UniformValue
    {
        protected int location;
        public UniformValue(string name, int shaderHandle)
        {
            location = GetUniformLocation(shaderHandle, name);
        }
        public abstract void upload(object value);
    }
    internal class UniformFloat : UniformValue
    {
        public UniformFloat(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform1(location, (float)value);
        }
    }
    internal class UniformInteger : UniformValue
    {
        public UniformInteger(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform1(location, (int)value);
        }
    }
    internal class UniformVec4 : UniformValue
    {
        public UniformVec4(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform4(location, (Vector4)value);
        }
    }
    internal class UniformMatrix4 : UniformValue
    {
        public UniformMatrix4(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Matrix4 matrix4 = (Matrix4)value;
            GL.UniformMatrix4(location, false, ref matrix4);
        }
    }
    internal class UniformVec3 : UniformValue
    {
        public UniformVec3(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform3(location, (Vector3)value);
        }
    }
    internal class UniformVec2 : UniformValue
    {
        public UniformVec2(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform2(location, (Vector2)value);
        }
    }
    internal class UniformSampler : UniformValue
    {
        public UniformSampler(string name, int shaderProgramHandle) : base(name, shaderProgramHandle) { }
        public override void upload(object value)
        {
            Uniform1(location, (int)value);
        }
    }
    public abstract class ShaderProgram
    {
        //might need this stuff later i guess idk
        protected string shaderName, vertexSource, fragmentSource;

        //very important to keep this though
        protected int shaderHandle;

        //compile and load the shader in the constructor bc why not
        public ShaderProgram(string shaderName)
        {
            this.shaderName = shaderName;
            init(shaderName,shaderName);
        }
        void init(string vertName, string fragName)
        {

            //Load in the source files
            vertexSource = File.ReadAllText("res/shaders/" + vertName + ".vert");
            fragmentSource = File.ReadAllText("res/shaders/" + fragName + ".frag");

            //Create the vertex shader on the GPU and get the handle
            int vertexShaderHandle = CreateShader(ShaderType.VertexShader);

            //load and compile it
            ShaderSource(vertexShaderHandle, vertexSource);
            CompileShader(vertexShaderHandle);

            //check for errors bc yk i will need it
            string vertexError = GetShaderInfoLog(vertexShaderHandle);
            if (vertexError != string.Empty)
            {
                Console.Error.WriteLine(vertexError);
            }

            //same for the fragment shader!
            int fragmentShaderHandle = CreateShader(ShaderType.FragmentShader);

            ShaderSource(fragmentShaderHandle, fragmentSource);
            CompileShader(fragmentShaderHandle);

            //gotta check for errors ofc!
            string fragmentError = GetShaderInfoLog(fragmentShaderHandle);
            if (fragmentError != string.Empty)
            {
                Console.Error.WriteLine(fragmentError);
            }

            //the shader program containing both vertex and fragment shaders!
            shaderHandle = CreateProgram();
            AttachShader(shaderHandle, vertexShaderHandle);
            AttachShader(shaderHandle, fragmentShaderHandle);

            LinkProgram(shaderHandle);

            //shaders already linked we can dispose of them now ig
            DetachShader(shaderHandle, vertexShaderHandle);
            DetachShader(shaderHandle, fragmentShaderHandle);

            DeleteShader(vertexShaderHandle);
            DeleteShader(fragmentShaderHandle);
        }
        public ShaderProgram(string vertName, string fragName)
        {
            shaderName = vertName;
            init(vertName, fragName);
        }

        public void bind()
        {
            UseProgram(shaderHandle);
        }
        public void unbind()
        {
            UseProgram(0);
        }
        /// <summary>
        /// oh didnt know i could do that
        /// 
        /// so like variables can be uploaded to the GPU here, usually before being used to render smth
        /// </summary>
        public abstract void uploadUniforms();

        //acab even the memory police
        public void cleanUp()
        {
            UseProgram(0);
            DeleteProgram(shaderHandle);
        }
    }
}
