using KaomojiFighters.Scenes.AlchemyScene;
using KaomojiFighters.Scenes.SellerScene;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Nez;

namespace KaomojiFighters.Scenes
{
    class ShopScene : Scene
    {
        TiledMapRenderer Background;
        private bool IsAlch;

        public ShopScene(bool alchemy)
        {
            IsAlch = alchemy;
            Background = CreateEntity("ShopBackground").SetScale(8).SetPosition(0, -1280).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("ShopMap")));
            Background.RenderLayer = 100;
            CreateEntity("Shop").AddComponent(new Shop(alchemy));
        }

        public override void OnStart()
        {
            base.OnStart();
            if (IsAlch) MediaPlayer.Play(Content.Load<Song>("Magic Lantern - Feasting on Energy (mp3cut.net)"));
            else MediaPlayer.Play(Content.Load<Song>("Crowander - Toy Shop (mp3cut.net)"));
        }

        public override void Unload()
        {
            MediaPlayer.Stop();
            base.Unload();
        }
    }
}
