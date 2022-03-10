using System.Collections.Generic;
using KaomojiFighters.Mobs;
using Nez;
using Nez.Sprites;
using KaomojiFighters.Scenes.DuelMode;
using KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            CreateEntity("VSSign").SetScale(0.6f).SetPosition(900, 175).AddComponent(new SpriteRenderer(Content.LoadTexture("VS")).SetRenderLayer(0));
            CreateEntity("BackgroundClouds").SetScale(2).SetPosition(0, 270).AddComponent(new Background() { BackgroundImageName = "ArenaBackgroundClouds" });
            CreateEntity("Kaomoji01").SetPosition(600, 700).AddComponent(new Player());
            CreateEntity("Kaomoji02").SetPosition(1400, 700).AddComponent(new Opponent());
            CreateEntity("Kaomoji02HealthBar").SetPosition(570, 175).AddComponent(new HealthBar() { entity = FindEntity("Kaomoji01") });
            CreateEntity("Kaomoji02HealthBar").SetPosition(570 + 710, 175).AddComponent(new HealthBar() { entity = FindEntity("Kaomoji02") });
            CreateEntity("tach").AddComponent(new Player1BattleHUD());
            GetOrCreateSceneComponent<SpeedoMeter>();
           
        }

    }
}
