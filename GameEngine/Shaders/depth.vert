#version 410 core

layout(location = 0) in vec3 aPos;

uniform mat4 lightview;
uniform mat4 lightprojection;
uniform mat4 model;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * lightview * lightprojection;
}