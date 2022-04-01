using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Scenes.SellerScene
{
    class ShopHUD : Component, IUpdatable
    {
        VirtualButton ExitShop;
        public Vector2 returnPosition;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ExitShop = new VirtualButton().AddKeyboardKey(Keys.Back);
        }

        public void Update()
        {
            if (ExitShop.IsPressed)
            {
                Core.StartSceneTransition(new TextureWipeTransition(() => new OverworldScene(returnPosition) , Entity.Scene.Content.LoadTexture("c")));
            }
        }
    }
}
