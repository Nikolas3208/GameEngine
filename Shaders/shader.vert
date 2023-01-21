#version 400 core

in vec3 aPos;
in vec3 aNormal;
in vec3 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
}