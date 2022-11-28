using System.Reflection.Emit;
using KaomojiFighters.Mobs;
using KaomojiFighters.Mobs.EnemyComponents;
using Nez;
using KaomojiFighters.Scenes.DuelMode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace KaomojiFighters
{
    class Battle : Scene
    {
        TiledMapRenderer Background;
        protected Mob opponent;
        

        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            TelegramService.DeregisterAll();
            Background = CreateEntity("BackgroundClouds").SetScale(3).SetPosition(0, 270).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("KaomojiDisplayMap")));
            Background.Transform.Position = new Vector2(0f,0f);
            Background.RenderLayer = 100;
            CreateEntity("Kaomoji01").SetPosition(600, 600).AddComponent(new Player());
            GetOpponent();
            CreateEntity("HUD").AddComponent(new HUD());

            var safe = new SpeedoMeter.ChangeLog (ChangeSafeFile);
            AddSceneComponent(new SpeedoMeter(safe));
        }

        protected virtual void GetOpponent()=> opponent = CreateEntity("Kaomoji02").SetPosition(1400, 600).AddComponent(new Opponent());

        public virtual void ChangeSafeFile()
        {

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

    class BossBattle: Battle
    {
        protected override void GetOpponent() => opponent = CreateEntity("Kaomoji02").SetPosition(1400, 600).AddComponent(new BossOpponent());

        public override void ChangeSafeFile() 
        {
            if (opponent.stat.HP <= 0)
            {
                SafeFileLoader.SaveOwOWorldSafeFile(new int[] { 0 });
            }
        }
    }
}
