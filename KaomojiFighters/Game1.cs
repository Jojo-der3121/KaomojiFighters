using KaomojiFighters.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters
{
    public class Game1 : Nez.Core
    {
        public Game1()
        {
            
        }
        protected override void Initialize()
        {
            base.Initialize();
            Screen.SetSize(1920, 1080);
            Scene.SetDefaultDesignResolution(1920, 1080, Scene.SceneResolutionPolicy.BestFit);
            Screen.SynchronizeWithVerticalRetrace = true;
            Window.AllowUserResizing = true;
            PauseOnFocusLost = false;
            //DebugRenderEnabled = true;
            var scene = new Battle();
            Scene = scene;
        }
    }
}
