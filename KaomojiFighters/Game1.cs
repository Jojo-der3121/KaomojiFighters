using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters
{
    public class Game1 : Nez.Core
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            Screen.SetSize(1920, 1080);
            Window.AllowUserResizing = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
            var scene = new Battle();
            Core.Scene = scene;
        }
    }
}
