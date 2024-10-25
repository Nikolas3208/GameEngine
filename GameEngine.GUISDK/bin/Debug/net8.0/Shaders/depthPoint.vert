#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 model;
uniform mat4 scale;
uniform mat4 rotation;

void main()
{
    gl_Position = model * rotation * scale * vec4(aPos, 1.0);
}