using KaomojiFighters.Scenes.AlchemyScene;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private SpriteRenderer selectionButton;
        private int selectionDestination;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private readonly SpeechBubble BuY;
        private readonly SpeechBubble Sell;
        private readonly SpeechBubble Leave;
        private ShopHUD shopHUD;
        private AlchemyHUD alchemyHUD;
        private AGenerateHUD aGenerateHUD;
        public int Gold;
        private bool _isAlchemy;

        public Shop(bool IsAlchemy)
        {
            _isAlchemy = IsAlchemy;
            BuY = new SpeechBubble(new Vector2(Screen.Center.X, 940), _isAlchemy? "Alchemy" : "BuY" , new Vector2(300, 220), true, _isAlchemy ? 5:7);
            Sell = new SpeechBubble(new Vector2(Screen.Center.X - 330, 940), _isAlchemy ? "Generate" : "Sell", new Vector2(300, 220), true, _isAlchemy ? 5 : 7);
            Leave = new SpeechBubble(new Vector2(Screen.Center.X + 330, 940), "Leave", new Vector2(300, 220), true, _isAlchemy ? 5 : 7);

        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ShopInner = Entity.Scene.Content.LoadTexture("shopInside");
            ShopOuter = Entity.Scene.Content.LoadTexture(_isAlchemy ? "AlchemyOutside" : "shopOutside");
            Seller = Entity.Scene.Content.LoadTexture("shopKoamoji");
            if (!_isAlchemy) DogdeCoin = Entity.Scene.Content.LoadTexture("R");

            // defines selection Button
            selectionButton = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(330f, 315f);
            selectionButton.RenderLayer = -1;
            selectionButton.LayerDepth = 0;
            selectionButton.Transform.Position = new Vector2(Screen.Center.X, 970f);
            selectionDestination = 0;
            selectionButton.Color = Color.White;

            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            if (_isAlchemy)
            {
                alchemyHUD = Entity.AddComponent(new AlchemyHUD());
                alchemyHUD.Enabled = false;
                aGenerateHUD = Entity.AddComponent(new AGenerateHUD());
                aGenerateHUD.Enabled = false;
            }
            else
            {
                shopHUD = Entity.AddComponent(new ShopHUD(this));
                shopHUD.Enabled = false;
            }

            var inn = Entity.AddComponent(new SpriteRenderer(ShopInner));
            inn.LocalOffset = Screen.Center - Entity.Position;
            inn.Size = new Vector2(1920 / 3, 1080);
            inn.RenderLayer = 1;
            var outer = Entity.AddComponent(new SpriteRenderer(ShopOuter));
            outer.LocalOffset = Screen.Center - Entity.Position;
            outer.Size = new Vector2(1920 / 3, 1080);
            outer.RenderLayer = 1;

        }
        protected override void Render(Batcher batcher, Camera camera)
        {
            //batcher.Draw(ShopInner, new RectangleF(Screen.Center.X - ShopInner.Width / 2, 0, 1920 / 3, 1080));
            batcher.Draw(Seller, new RectangleF(Screen.Center.X - Seller.Width / 4, Screen.Center.Y, 400, 125));
            //batcher.Draw(ShopOuter, new RectangleF(Screen.Center.X - ShopOuter.Width / 2, 0, (1920 / 3) * (7 / 6), 1080 * (7 / 6)));
            SetRenderLayer(-1);
            BuY.DrawTextField(batcher);
            Sell.DrawTextField(batcher);
            Leave.DrawTextField(batcher);
            if (!_isAlchemy)
            {
                batcher.DrawRect(new RectangleF(1120, 0, 310, 80), Color.Black);
                batcher.DrawRect(new RectangleF(1130, 0, 290, 70), Color.DarkGreen);
                batcher.DrawString(Graphics.Instance.BitmapFont, Gold.ToString(), new Vector2(1200, 5f), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
                batcher.Draw(DogdeCoin, new RectangleF(1140, 5, 50, 50));
            }
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        public void Update()
        {
            if (selectionDestination - 330 >= -330 && Left.IsPressed && (_isAlchemy?  !alchemyHUD.Enabled && !aGenerateHUD.Enabled : !shopHUD.Enabled)  && selectionButton.Enabled)
            {
                selectionDestination -= 330;
            }
            if (selectionDestination + 330 <= 330 && Right.IsPressed && (_isAlchemy ? !alchemyHUD.Enabled && !aGenerateHUD.Enabled : !shopHUD.Enabled) && selectionButton.Enabled)
            {
                selectionDestination += 330;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);

            if (!Enter.IsReleased) return;
            switch (selectionDestination)
            {
                case -330:
                    if (_isAlchemy)
                    {
                        aGenerateHUD.Enabled = true;
                    }
                    else
                    {
                        shopHUD.buy = false;
                        shopHUD.Enabled = true;
                    }
                    break;
                case 0:
                    if (_isAlchemy)
                    {
                        alchemyHUD.Enabled = true;
                    }
                    else
                    {
                        shopHUD.buy = true;
                        shopHUD.Enabled = true;
                    }
                    break;
                case 330:
                    Core.StartSceneTransition(new TextureWipeTransition(() => new OverworldScene(SafeFileLoader.LoadStats().OwOworldPosition), Entity.Scene.Content.LoadTexture("c")));
                    break;

            }
        }
    }
}

