#version 410  core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    vec3 fragPos = vec3(vec4(aPos, 1.0) * model);
    gl_Position = vec4(fragPos, 1.0) * view * projection;
}