using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Textures;
using Nez.Sprites;
using Nez.UI;

namespace KaomojiFighters.Scenes
{
    class MenuScene : Scene

    {
        VirtualButton Start;
        VirtualButton Exit;
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            Start = new VirtualButton().AddKeyboardKey(Keys.Space);
            Exit = new VirtualButton().AddKeyboardKey(Keys.Escape);
            CreateEntity("TitelScrean").SetLocalPosition(Screen.Center).AddComponent(new SpriteRenderer(Content.LoadTexture("ExplosionKaoUwu")));
        }

        public override void Update()
        {
            base.Update();
            if (Start.IsPressed)
            {
                Core.StartSceneTransition(new TextureWipeTransition(() => new Battle(), Content.LoadTexture("nez/textures/textureWipeTransition/pokemon")));
            }
            if (Exit.IsPressed)
            {
                Core.Exit();
            }
        }
    }
}
