using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters
{
    class Background : Component, IUpdatable, ITelegramReceiver
    {
        public string BackgroundImageName;
        private SpriteRenderer _background;
        private TiledMapRenderer Display;
     

        public void MessageReceived(Telegram message)
        {
            if (message.Head == "Frohe Ostern")
            {
                _background.Sprite = new Sprite(Entity.Scene.Content.LoadTexture(BackgroundImageName + "Akatsuki"));
            }
        }

        public override void OnAddedToEntity()
        {
            
            TelegramService.Register(this, Entity.Name);
            _background = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(BackgroundImageName)));
            Display = Entity.AddComponent(new TiledMapRenderer(Entity.Scene.Content.LoadTiledMap("KaomojiDisplayMap")));
            Display.RenderLayer = 1;
            Display.LayerDepth = 0.1f;
            Display.Transform.SetScale(3f);
            Display.Transform.SetPosition(new Vector2(100,50));
            Entity.AddComponent(new EasterEgg() { EasterEggString = new Keys[] { Keys.A,Keys.K,Keys.A,Keys.T,Keys.S,Keys.U,Keys.K,Keys.I } });
            _background.RenderLayer = 1;
            _background.LayerDepth = 0.2f;
        }

        public void Update()
        {
            _background.LocalOffset = new Vector2(_background.LocalOffset.X + 100 * Time.DeltaTime, _background.LocalOffset.Y);
            if (_background.LocalOffset.X >= 1920)
            {
                _background.LocalOffset = new Vector2(0, _background.LocalOffset.Y);
            }
           
        }
    }
}
