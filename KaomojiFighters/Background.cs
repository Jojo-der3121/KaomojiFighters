using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters
{
    class Background : Component, IUpdatable
    {
        public string BackgroundImageName;
        private SpriteRenderer _background;
        private TiledMapRenderer Display;
        private Scene scene;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            _background = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture(BackgroundImageName)));
            Display = Entity.AddComponent(new TiledMapRenderer(scene.Content.LoadTiledMap("KaomojiDisplayMap")));
            Entity.AddComponent(new AkatsukiEasterEgg());
            _background.RenderLayer = 1;
            _background.LayerDepth = 0.2f;
        }

        public void Update()
        {
            Entity.Position = new Vector2(Entity.Position.X + 100 * Time.DeltaTime, Entity.Position.Y);
            if (Entity.Position.X >= 1920)
            {
                Entity.Position = new Vector2(0, Entity.Position.Y);
            }
        }
    }
}
