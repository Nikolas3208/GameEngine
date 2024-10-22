#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec3 aTexCoords;

uniform mat4 model;
uniform mat4 scale;
uniform mat4 rotation;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightview;
uniform mat4 lightprojection;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;
out vec4 LightSpacePos;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * rotation * scale * view * projection;
    FragPos = vec3(vec4(aPos, 1.0) * model * rotation * scale);
    Normal = aNormal * mat3(transpose(inverse(model)));
    TexCoords = aTexCoords.xy;
    LightSpacePos = vec4(aPos, 1.0) * model * rotation * scale * lightview * lightprojection;
}
