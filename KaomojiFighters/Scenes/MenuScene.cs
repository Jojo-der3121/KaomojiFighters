using Microsoft.Xna.Framework.Input;
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
