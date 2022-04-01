using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Scenes.SellerScene;
using Microsoft.Xna.Framework;
using Nez;

namespace KaomojiFighters.Scenes
{
    class ShopScene: Scene
    {
        TiledMapRenderer Background;
       
        public ShopScene(Vector2 PriorPlayerPosition)
        {
            Background = CreateEntity("ShopBackground").SetScale(8).SetPosition(0,-1280).AddComponent(new TiledMapRenderer(Content.LoadTiledMap("ShopMap")));
            Background.RenderLayer = 100;
            CreateEntity("Shop").AddComponent(new Shop());
            CreateEntity("ShopLogic").AddComponent(new ShopHUD() {returnPosition = PriorPlayerPosition });
        }
    }
}
