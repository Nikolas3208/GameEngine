#version 410

in vec2 TexCoords;

struct Material
{
    sampler2D deffuse;

    vec3 Color;
};

uniform Material material;

out vec4 FragColor;

void main()
{
    vec3 result = vec3(texture(material.deffuse, TexCoords)) * material.Color;
    FragColor = vec4(result, 1.0);
}