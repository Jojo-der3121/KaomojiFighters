using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs;
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
        Texture2D DogdeCoin;
        public SpriteRenderer selectionButton;
        private int selectionDestination;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private SpeechBubble BuY = new SpeechBubble(new Vector2(Screen.Center.X, 940),"BuY", new Vector2(300,220),true, 7);
        private SpeechBubble Sell = new SpeechBubble(new Vector2(Screen.Center.X - 330, 940),"Sell", new Vector2(300,220),true,7);
        private SpeechBubble Leave = new SpeechBubble(new Vector2(Screen.Center.X + 330, 940), "Leave", new Vector2(300, 220), true,7);
        private ShopHUD shopHUD;
        public int Gold;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ShopInner = Entity.Scene.Content.LoadTexture("shopInside");
            ShopOuter = Entity.Scene.Content.LoadTexture("shopOutside");
            Seller = Entity.Scene.Content.LoadTexture("shopKoamoji");
            DogdeCoin = Entity.Scene.Content.LoadTexture("R");

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

            shopHUD = Entity.AddComponent(new ShopHUD(this));
            shopHUD.Enabled = false;
        }
        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(ShopInner, new RectangleF(Screen.Center.X - ShopInner.Width/2, 0, 1920 / 3, 1080));
            batcher.Draw(Seller, new RectangleF(Screen.Center.X - Seller.Width/4, Screen.Center.Y , 400, 125));
            batcher.Draw(ShopOuter, new RectangleF(Screen.Center.X - ShopOuter.Width/2, 0, (1920 / 3)*(7/6), 1080*(7/6)));
            BuY.DrawTextField(batcher);
            Sell.DrawTextField(batcher);
            Leave.DrawTextField(batcher);
            batcher.DrawRect(new RectangleF(1120, 0, 310, 80), Color.Black);
            batcher.DrawRect(new RectangleF( 1130, 0, 290,70), Color.DarkGreen);
            batcher.DrawString(Graphics.Instance.BitmapFont, Gold.ToString(), new Vector2(1200, 5f), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            batcher.Draw(DogdeCoin,new RectangleF(1140, 5, 50,50 ));
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

