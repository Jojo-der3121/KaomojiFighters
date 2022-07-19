using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Scenes
{
    class MenuScene : Scene

    {
        VirtualButton Start;
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            Start = new VirtualButton().AddKeyboardKey(Keys.Space);
            CreateEntity("TitelScrean").SetLocalPosition(Screen.Center).AddComponent(new SpriteRenderer(Content.LoadTexture("ExplosionKaoUwu")));
        }

        public override void OnStart()
        {
            base.OnStart();
            MediaPlayer.Play(Content.Load<Song>("old-desktop-pc-booting-24280 (mp3cut.net)"));
            MediaPlayer.IsRepeating = true;
        }

        public override void Unload()
        {
            base.Unload();
            MediaPlayer.Stop();
        }

        public override void Update()
        {
            base.Update();
            if (Start.IsPressed)
            {
                Core.StartSceneTransition(new TextureWipeTransition(() => new OverworldScene(), Content.LoadTexture("c"))); // OverworldScene
            }
        }
    }
}
