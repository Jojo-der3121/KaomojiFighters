using System.Collections.Generic;
using KaomojiFighters.Mobs;
using Nez;
using Nez.Sprites;
using KaomojiFighters.Scenes.DuelMode;

using Microsoft.Xna.Framework;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        TiledMapRenderer Background;
        
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            Background = CreateEntity("BackgroundClouds").SetScale(3).SetPosition(0, 270).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("KaomojiDisplayMap")));
            Background.Transform.Position = new Vector2(0f,0f);
            Background.RenderLayer = 100;
            CreateEntity("Kaomoji01").SetPosition(600, 600).AddComponent(new Player());
            CreateEntity("Kaomoji02").SetPosition(1400, 600).AddComponent(new Opponent());
            CreateEntity("HUD").AddComponent(new HUD());
           
            GetOrCreateSceneComponent<SpeedoMeter>();
           
        }

    }
}
