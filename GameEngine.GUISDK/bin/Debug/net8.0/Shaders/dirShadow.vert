#version 330  core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aTexCoords;

uniform mat4 model;
uniform mat4 rotation;
uniform mat4 scale;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightview;
uniform mat4 lightprojection;

out DATA
{
    vec2 TexCoords;
    vec3 Normal;
    vec3 FragPos;
    vec4 LightSpacePos;
} data_out;




void main()
{
    data_out.FragPos = vec3(vec4(aPos, 1.0) * rotation * scale * model);
    gl_Position = vec4(data_in.FragPos, 1.0) * view * projection;
    data_out.Normal = aNormal * mat3(transpose(inverse(rotation * scale * model)));
    data_out.TexCoords = aTexCoords.xy;
    data_out.LightSpacePos = vec4(data_in.FragPos,1.0) * lightview * lightprojection;
}