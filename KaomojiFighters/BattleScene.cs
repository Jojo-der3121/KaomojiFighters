using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Textures;
using Nez.Sprites;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            CreateEntity("Kaomoji01").SetPosition(Screen.Center).AddComponent(new Punch()).AddComponent(new WASDMovement());
            CreateEntity("Kaomoji02").SetPosition(1700, 700).AddComponent(new SpriteRenderer(Content.LoadTexture("Kaomoji02"))).AddComponent(new FollowPlayer() { LerpIndex = 0.02f });
        }
    }
}
