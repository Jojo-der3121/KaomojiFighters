using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Scenes.SellerScene
{

    class Shop : RenderableComponent
    {
        Texture2D ShopInner;
        Texture2D ShopOuter;
        Texture2D Seller;
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ShopInner = Entity.Scene.Content.LoadTexture("shopInside");
            ShopOuter = Entity.Scene.Content.LoadTexture("shopOutside");
            Seller = Entity.Scene.Content.LoadTexture("shopKoamoji");

        }
        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(ShopInner, new RectangleF(Screen.Center.X - ShopInner.Width/2, 0, 1920 / 3, 1080));
            batcher.Draw(Seller, new RectangleF(Screen.Center.X - Seller.Width/4, Screen.Center.Y , 400, 125));
            batcher.Draw(ShopOuter, new RectangleF(Screen.Center.X - ShopOuter.Width/2, 0, (1920 / 3)*(7/6), 1080*(7/6)));
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

    }
}

