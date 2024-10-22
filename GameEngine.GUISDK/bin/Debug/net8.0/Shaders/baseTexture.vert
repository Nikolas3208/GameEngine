#version 410 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aTexCoords;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 model;
uniform mat4 rotation;
uniform mat4 scale;

out vec2 TexCoords;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * scale * rotation * view * projection;
    TexCoords = aTexCoords.xy;
}