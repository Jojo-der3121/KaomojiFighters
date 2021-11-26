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
using Nez.UI;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            var VSsign = CreateEntity("VSSign").SetScale(0.8f).SetPosition(900, 75).AddComponent(new SpriteRenderer(Content.LoadTexture("VS")).SetRenderLayer(0));
            VSsign.RenderLayer = 0;
            CreateEntity("BackgroundClouds").SetScale(2).SetPosition(0,270).AddComponent(new Background(){ BackgroundImageName = "ArenaBackgroundClouds" });
            var player = CreateEntity("Kaomoji01").SetPosition(300,700).AddComponent(new Player());
            var enemy = CreateEntity("Kaomoji02").SetPosition(1700, 700).AddComponent(new Opponent());
            player.AddComponent(new PlayerHitBox());
            CreateEntity("Kaomoji02HealthBar").SetPosition(460+1000,75).AddComponent(new HealthBar() {entity = FindEntity("Kaomoji02")});
            CreateEntity("Kaomoji02HealthBar").SetPosition(460,75).AddComponent(new HealthBar() {entity = FindEntity("Kaomoji01")});
        }
    }
}
