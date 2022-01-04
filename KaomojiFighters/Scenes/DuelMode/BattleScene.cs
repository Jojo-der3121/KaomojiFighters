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
using KaomojiFighters.Scenes.DuelMode;
using KaomojiFighters.Scenes;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        private Player player1;
        private Opponent enemy;
        public List<Stats> battlingEntities;
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            battlingEntities = new List<Stats>();
            var VSsign = CreateEntity("VSSign").SetScale(0.6f).SetPosition(900, 175).AddComponent(new SpriteRenderer(Content.LoadTexture("VS")).SetRenderLayer(0));
            VSsign.RenderLayer = 0;
            CreateEntity("BackgroundClouds").SetScale(2).SetPosition(0, 270).AddComponent(new Background() { BackgroundImageName = "ArenaBackgroundClouds" });
            player1 = CreateEntity("Kaomoji01").SetPosition(600, 700).AddComponent(new Player());
            enemy = CreateEntity("Kaomoji02").SetPosition(1400, 700).AddComponent(new Opponent());
            player1.AddComponent(new PlayerHitBox());
            CreateEntity("Kaomoji02HealthBar").SetPosition(570, 175).AddComponent(new HealthBar() { entity = FindEntity("Kaomoji01") });
            CreateEntity("Kaomoji02HealthBar").SetPosition(570 + 710, 175).AddComponent(new HealthBar() { entity = FindEntity("Kaomoji02") });
        }

    }
}
