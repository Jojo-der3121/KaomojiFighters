using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters
{
    class Background : Component, IUpdatable
    {
        public string BackgroundImageName;
        private SpriteRenderer _background;
        private TiledMapRenderer Display;
        private Scene scene;
        private EasterEgg easterEgg;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            _background = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture(BackgroundImageName)));
            Display = Entity.AddComponent(new TiledMapRenderer(scene.Content.LoadTiledMap("KaomojiDisplayMap")));
            Display.RenderLayer = 1;
            Display.LayerDepth = 0.1f;
            Display.Transform.SetScale(3f);
            Display.Transform.SetPosition(new Vector2(100,50));
            easterEgg = Entity.AddComponent(new EasterEgg() { EasterEggString = new Keys[] { Keys.A,Keys.K,Keys.A,Keys.T,Keys.S,Keys.U,Keys.K,Keys.I } });
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
            if (easterEgg.IsActivated)
            {
                _background.Sprite = new Sprite(scene.Content.LoadTexture(BackgroundImageName + "Akatsuki"));
                easterEgg.IsActivated = false;
            }
        }
    }
}
