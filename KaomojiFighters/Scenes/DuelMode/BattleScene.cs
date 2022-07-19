using KaomojiFighters.Mobs;
using Nez;
using KaomojiFighters.Scenes.DuelMode;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        TiledMapRenderer Background;

        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            TelegramService.DeregisterAll();
            Background = CreateEntity("BackgroundClouds").SetScale(3).SetPosition(0, 270).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("KaomojiDisplayMap")));
            Background.Transform.Position = new Vector2(0f,0f);
            Background.RenderLayer = 100;
            CreateEntity("Kaomoji01").SetPosition(600, 600).AddComponent(new Player());
            CreateEntity("Kaomoji02").SetPosition(1400, 600).AddComponent(new Opponent());
            CreateEntity("HUD").AddComponent(new HUD());
           
            GetOrCreateSceneComponent<SpeedoMeter>();
           
        }

        public override void OnStart()
        {
            base.OnStart();
            MediaPlayer.Play(Content.Load<Song>("PC-ONE - Monachine (mp3cut.net)"));
        }

        public override void Unload()
        {
            MediaPlayer.Stop();
            base.Unload();
        }

    }
}
