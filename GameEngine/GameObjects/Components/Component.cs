﻿using GameEngine.Core.Essentials;
using GameEngine.GameObjects.Components.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameEngine.GameObjects.Components
{
    [JsonDerivedType(typeof(CameraRender), 0)]
    [JsonDerivedType(typeof(TransformComponet), 1)]
    [JsonDerivedType(typeof(LightRender), 2)]
    [JsonDerivedType(typeof(MeshRender), 3)]
    public class Component
    {
        private GameObject gameObject;

        [JsonInclude]
        public GameObject GameObject { get => gameObject; set { gameObject = value; gameObjectId = value.Id; } }

        public int gameObjectId;

        [JsonInclude]
        public string Name = "Component";

        public virtual void Start()
        {

        }
        public virtual void Update(float deltaTime)
        {

        }
        public virtual void Draw(Shader shader)
        {

        }
    }
}
