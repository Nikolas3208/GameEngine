#version 410  core

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
    mat4 model;
    vec3 Normal;
    vec3 FragPos;
    vec2 TexCoords2;
    vec4 LightSpacePos;
    mat4 view;
} data_in;




void main()
{
    data_in.FragPos = vec3(vec4(aPos, 1.0) * rotation * scale * model);
    gl_Position = vec4(data_in.FragPos, 1.0);
    data_in.Normal = aNormal * mat3(transpose(inverse(rotation * scale * model)));
    data_in.TexCoords2 = aTexCoords.xy;
    data_in.model = rotation * scale * model;
    data_in.LightSpacePos = vec4(data_in.FragPos,1.0) * lightview * lightprojection;
    data_in.view = view * projection;
}