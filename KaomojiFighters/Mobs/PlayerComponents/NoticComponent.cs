using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace KaomojiFighters.Mobs.PlayerComponents
{
    class NoticComponent: RenderableComponent
    {
        private OwOWorldPlayer player;

        public NoticComponent(OwOWorldPlayer p)
        {
            player = p;
        }
        protected override void Render(Batcher batcher, Camera camera)
        {
            if (player.inTrigger) batcher.DrawString(Graphics.Instance.BitmapFont, "press E to interact", new Vector2(Screen.Size.X - 200, 300), Color.White, 0f, Vector2.Zero, 10, SpriteEffects.None, 0f);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;
    }
}
