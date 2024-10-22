#version 410  core

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D depthMap;
    sampler2D normal;
    vec3 Color;
    float textureScale;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    vec3 color;

    float constant;
    float linear;
    float quadratic;
    float cutOff;

    int type;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;

out vec4 FragColor;

in VS
{
 vec3 Normal;
 vec3 FragPos;
 vec2 TexCoords;
 vec4 LightSpacePos;
} vs_in;

uniform sampler2D shadowMap;
uniform float height_scale;

in mat3 TBN;

uniform int line;

float ShadowCalculation(vec4 lightSpacePos,vec3 norm, vec3 lightDir)
{
    vec3 projCoords = lightSpacePos.xyz / lightSpacePos.w;
    
    projCoords = projCoords * 0.5 + 0.5;
    
    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    
    float currentDepth = projCoords.z;
    
    float bias = max(0.05 * (1.0 - dot(norm, lightDir)), 0.005);

    //float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
//    // PCF
    float shadow = 0.0;

    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }

    shadow /= 9.0;

    if(projCoords.z > 1.0)
        shadow = 0.0;

    return shadow;
}

vec3 PointLight(Light light, vec3 fragPos, vec3 lightDir, vec3 viewDir, vec3 normal, vec2 texCoords)
{
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoords));

    // Diffuse 
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, texCoords))) * material.Color;

    // Specular
    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * vec3(texture(material.specular, texCoords)));

    vec3 result = ambient + diffuse + specular;

    return result;
}

vec3 DirectionalLight(Light light, vec2 texCoords, vec3 viewDir, vec3 lightDir, vec3 normal)
{
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoords));

    // Diffuse 
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, texCoords)) ;

    // Specular
    vec3 reflectDir = reflect(-lightDir, normal);
    vec3 a = normalize(lightDir + reflectDir);
    float spec = pow(max(dot(normal, a), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, texCoords));

    vec3 result = ambient + diffuse + specular;

    return result;
}

vec3 Spotlight(vec3 norm)
{
    vec3 lightDir = normalize(light.position - vs_in.FragPos);
    float theta = dot(lightDir, normalize(-light.direction));
    
    if(theta > light.cutOff) 
    {       
        vec3 ambient = light.ambient * vec3(texture(material.diffuse, vs_in.TexCoords));

        // Diffuse 
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, vs_in.TexCoords));

        // Specular
        vec3 viewDir = normalize(viewPos - vs_in.FragPos);
        vec3 reflectDir = reflect(-lightDir, norm);
        vec3 halfwayDir = normalize(lightDir + viewDir);
        float spec = pow(max(dot(viewDir, halfwayDir), 0.0), material.shininess);
        vec3 specular = light.specular * spec * vec3(texture(material.specular, vs_in.TexCoords));

        vec3 result = ambient + diffuse + specular;

        return result;
    }
    else
    {
        return light.ambient * vec3(texture(material.diffuse, vs_in.TexCoords));
    }

    return vec3(1);
}

vec2 ParallaxMapping(vec2 texCoords, vec3 viewDir)
{ 
        // number of depth layers
    const float minLayers = 8;
    const float maxLayers = 32;
    float numLayers = mix(maxLayers, minLayers, abs(dot(vec3(0.0, 0.0, 1.0), viewDir)));  
    // calculate the size of each layer
    float layerDepth = 1.0 / numLayers;
    // depth of current layer
    float currentLayerDepth = 0.0;
    // the amount to shift the texture coordinates per layer (from vector P)
    vec2 P = viewDir.xy / viewDir.z * height_scale; 
    vec2 deltaTexCoords = P / numLayers;
  
    // get initial values
    vec2  currentTexCoords     = texCoords;
    float currentDepthMapValue = texture(material.depthMap, currentTexCoords).r;
      
    while(currentLayerDepth < currentDepthMapValue)
    {
        // shift texture coordinates along direction of P
        currentTexCoords -= deltaTexCoords;
        // get depthmap value at current texture coordinates
        currentDepthMapValue = texture(material.depthMap, currentTexCoords).r;  
        // get depth of next layer
        currentLayerDepth += layerDepth;  
    }
    
    // get texture coordinates before collision (reverse operations)
    vec2 prevTexCoords = currentTexCoords + deltaTexCoords;

    // get depth after and before collision for linear interpolation
    float afterDepth  = currentDepthMapValue - currentLayerDepth;
    float beforeDepth = texture(material.depthMap, prevTexCoords).r - currentLayerDepth + layerDepth;
 
    // interpolation of texture coordinates
    float weight = afterDepth / (afterDepth - beforeDepth);
    vec2 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);

    return finalTexCoords;  
} 


uniform int useFbo;
uniform int gameObjectId;

out int id;

void main()
{
    vec3 result = material.Color;
    
    vec3 lightDir = normalize(light.position - vs_in.FragPos);
    vec3 viewDir = normalize(viewPos - vs_in.FragPos);

    if(light.type == 1)
        result = PointLight(light, vs_in.FragPos, lightDir, viewDir, vs_in.Normal, vs_in.TexCoords * material.textureScale);
    if(light.type == 2)
    {
        lightDir = normalize(-light.direction);
        float shadow = ShadowCalculation(vs_in.LightSpacePos, vs_in.Normal, lightDir);
        result = DirectionalLight(light, vs_in.TexCoords, viewDir, lightDir, vs_in.Normal) * (1 - shadow);
    }
    if(light.type == 3)
    {
        result = Spotlight(vs_in.Normal);
    }
    
    if(useFbo == 0 && line == 0)
        {
            FragColor = vec4(result * material.Color, 1.0);
        }
    else if(line == 0)
        {
            FragColor = vec4(gameObjectId, 1,1, 1.0);
        }
        else if(line == 1)
            FragColor = vec4(1.0, 1.0, 0.0, 1.0);
}