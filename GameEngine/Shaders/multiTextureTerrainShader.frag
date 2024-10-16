#version 400 core

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float     shininess;
};
struct Light {
    vec3 position;
    vec3 lightDir;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

in vec3 Normal;
in vec3 FragPos;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D texture2;
uniform sampler2D texture3;
uniform sampler2D texture4;

in vec2 TexCoords;

out vec4 FragColor;

void main()
{
	vec4 blendMapColour = texture(texture4, TexCoords);

    float distance = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));


	float backTextureAmount = 1 - (blendMapColour.r + blendMapColour.g + blendMapColour.b);
	vec2 Coords = TexCoords * 40.0;

	vec4 color0 = texture(texture0, Coords) * backTextureAmount;
	vec4 rcolor = texture(texture1, Coords) * blendMapColour.r;
	vec4 gcolor = texture(texture2, Coords) * blendMapColour.g;
	vec4 bcolor = texture(texture3, Coords) * blendMapColour.b;

	vec4 color = color0 + rcolor + gcolor + bcolor;
	
	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords)) * attenuation;

    // Diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords)) * attenuation;

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords)) * attenuation;

    vec3 result = ambient + diffuse + specular;

	FragColor = color + vec4(result,1.0);
}