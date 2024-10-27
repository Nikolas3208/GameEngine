using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    [Serializable]
    public class Shader
    {
        protected int Handle;
        
        protected Dictionary<string, int> uniformLocations;

        public static Shader LoadFromFile(string path)
        {
            if (File.Exists(path + ".geom"))
            {
                return new Shader(path + ".vert", path + ".frag", path + ".geom");
            }
            else
            {
                return new Shader(path + ".vert", path + ".frag");
            }
        }

        public Shader(string vertPath, string fragPath, string geomPath = "")
        {

            int Handle = GL.CreateProgram();

            string shaderSource = File.ReadAllText(vertPath);
            int shaderVert = CreateShader(ShaderType.VertexShader, shaderSource);

            shaderSource = File.ReadAllText(fragPath);
            int shaderFrag = CreateShader(ShaderType.FragmentShader, shaderSource);

            int shaderGeom = -1;
            if (geomPath != "")
            {
                shaderSource = File.ReadAllText(geomPath);
                shaderGeom = CreateShader(ShaderType.GeometryShader, shaderSource);
            }

            GL.AttachShader(Handle, shaderVert);
            if (shaderGeom != -1)
                GL.AttachShader(Handle, shaderGeom);
            GL.AttachShader(Handle, shaderFrag);


            LinkProgram(Handle);

            GL.DetachShader(Handle, shaderFrag);
            GL.DetachShader(Handle, shaderVert);
            GL.DetachShader(Handle, shaderGeom);

            GL.DeleteShader(shaderFrag);
            GL.DeleteShader(shaderVert);
            GL.DeleteShader(shaderGeom);

            GetUniformsLocation(Handle);

            this.Handle = Handle;
        }

        protected void GetUniformsLocation(int handle)
        {
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int uniformsCount);

            uniformLocations = new Dictionary<string, int>();

            for (int i = 0; i < uniformsCount; i++)
            {
                string key = GL.GetActiveUniform(handle, i, out _, out _);

                int location = GL.GetUniformLocation(handle, key);

                uniformLocations.Add(key, location);
            }
        }

        protected void LinkProgram(int handle)
        {
            GL.LinkProgram(handle);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out var status);
            if (status != (int)All.True)
            {
                var log = GL.GetProgramInfoLog(handle);
                throw new Exception($"Error occurred whilst linking Program({handle}).\n\n{log}");
            }
        }

        protected int CreateShader(ShaderType shaderType, string source)
        {
            int shader = GL.CreateShader(shaderType);

            GL.ShaderSource(shader, source);

            CompileShader(shader);

            return shader;
        }

        protected void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var status);
            if (status != (int)All.True)
            {
                var log = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{log}");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return uniformLocations[name];
        }

        public bool ContainsKey(string name)
        {
            return uniformLocations.ContainsKey(name);
        }

        public void SetInt(string name, int value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.Uniform1(GetUniformLocation(name), value);
            }
        }

        public void SetFloat(string name, float value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.Uniform1(GetUniformLocation(name), value);
            }
        }

        public void SetVector3(string name, Vector3 value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.Uniform3(GetUniformLocation(name), value);
            }
        }

        public void SetVector3(string name, Core.Structs.Vector3f value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.Uniform3(GetUniformLocation(name), new Vector3(value.X, value.Y, value.Z));
            }
        }

        public void SetVector4(string name, Vector4 value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.Uniform4(GetUniformLocation(name), value);
            }
        }

        public void SetMatrix4(string name, Matrix4 value)
        {
            if (ContainsKey(name))
            {
                Use();
                GL.UniformMatrix4(GetUniformLocation(name), true, ref value);
            }
        }

        ~Shader()
        {
            //GL.DeleteProgram(Handle);
        }
    }
}
