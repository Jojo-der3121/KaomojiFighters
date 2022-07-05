using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez.BitmapFonts;
using Nez.Sprites;


namespace KaomojiFighters.Scenes.SellerScene
{

    class Shop : RenderableComponent, IUpdatable
    {
        Texture2D ShopInner;
        Texture2D ShopOuter;
        Texture2D Seller;
        public SpriteRenderer selectionButton;
        private int selectionDestination;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private ShopHUD shopHUD;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ShopInner = Entity.Scene.Content.LoadTexture("shopInside");
            ShopOuter = Entity.Scene.Content.LoadTexture("shopOutside");
            Seller = Entity.Scene.Content.LoadTexture("shopKoamoji");

            // defines selection Button
            selectionButton = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(330f, 315f);
            selectionButton.RenderLayer = -1;
            selectionButton.LayerDepth = 0;
            selectionButton.Transform.Position = new Vector2(Screen.Center.X , 970f);
            selectionDestination = 0;
            selectionButton.Color = Color.White;

            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);

            shopHUD = Entity.AddComponent(new ShopHUD());
            shopHUD.Enabled = false;
           

        }
        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(ShopInner, new RectangleF(Screen.Center.X - ShopInner.Width/2, 0, 1920 / 3, 1080));
            batcher.Draw(Seller, new RectangleF(Screen.Center.X - Seller.Width/4, Screen.Center.Y , 400, 125));
            batcher.Draw(ShopOuter, new RectangleF(Screen.Center.X - ShopOuter.Width/2, 0, (1920 / 3)*(7/6), 1080*(7/6)));
            
            batcher.DrawRect(Screen.Center.X -150, 830f , 300f,220f, Color.Black);
            batcher.DrawRect(Screen.Center.X - 140, 840f, 280f, 200f, new Color(104, 201, 52));
            batcher.DrawString(Graphics.Instance.BitmapFont, "BuY", new Vector2(Screen.Center.X- 75 , 880f), Color.Black, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0f);
            batcher.DrawRect(Screen.Center.X -150 -330, 830f , 300f,220f, Color.Black);
            batcher.DrawRect(Screen.Center.X - 140 -330, 840f, 280f, 200f, new Color(104, 201, 52));
            batcher.DrawString(Graphics.Instance.BitmapFont, "Sell", new Vector2(Screen.Center.X - 75 -330, 880f), Color.Black, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0f);
            batcher.DrawRect(Screen.Center.X -150 +330, 830f , 300f,220f, Color.Black);
            batcher.DrawRect(Screen.Center.X - 140 +330, 840f, 280f, 200f, new Color(104, 201, 52));
            batcher.DrawString(Graphics.Instance.BitmapFont, "Leave", new Vector2(Screen.Center.X - 75 +270, 880f), Color.Black, 0f, Vector2.Zero, 9f, SpriteEffects.None, 0f);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        public void Update()
        {
            if (selectionDestination - 330 >= -330 && Left.IsPressed && !shopHUD.Enabled &&  selectionButton.Enabled)
            {
                selectionDestination -= 330;
            }
            if (selectionDestination + 330 <= 330 && Right.IsPressed && !shopHUD.Enabled && selectionButton.Enabled)
            {
                selectionDestination += 330;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);

            bool ignoreShopHUDUpdate = false;
            bool ignoreSellHUDUpdate = false;
            if (Enter.IsPressed)
            {
                switch (selectionDestination)
                {
                    case -330:
                        shopHUD.Enabled = true;
                        shopHUD.buy = false;
                        break;
                    case 0:
                        shopHUD.Enabled = true;
                        shopHUD.buy = true;
                        ignoreShopHUDUpdate = true;
                        break;
                    case 330:
                        Core.StartSceneTransition(new TextureWipeTransition(() => new OverworldScene(SafeFileLoader.LoadStats().OwOworldPosition), Entity.Scene.Content.LoadTexture("c")));
                        break;

                }
            }
        }
    }
}

