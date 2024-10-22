#version 330 core

layout(location = 0) in vec3 aPos;

uniform mat4 lightview;
uniform mat4 lightprojection;
uniform mat4 model;
uniform mat4 scale;
uniform mat4 rotation;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * rotation * scale * lightview * lightprojection;
}