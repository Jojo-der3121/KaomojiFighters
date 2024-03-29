﻿using KaomojiFighters.Scenes;
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
            DebugRenderEnabled = false;
            var scene = new MenuScene();
            Scene = scene;
        }
    }
}
