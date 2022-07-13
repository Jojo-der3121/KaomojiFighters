using KaomojiFighters.Scenes.AlchemyScene;
using KaomojiFighters.Scenes.SellerScene;
using Nez;

namespace KaomojiFighters.Scenes
{
    class ShopScene : Scene
    {
        TiledMapRenderer Background;


        public ShopScene(bool alchemy)
        {
            Background = CreateEntity("ShopBackground").SetScale(8).SetPosition(0, -1280).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("ShopMap")));
            Background.RenderLayer = 100;
            CreateEntity("Shop").AddComponent(new Shop(alchemy));
        }
    }
}
