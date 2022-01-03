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
        VirtualButton Button;
        public override void Initialize()
        {
            AddRenderer(new DefaultRenderer());
            base.Initialize();
            Button = new VirtualButton().AddKeyboardKey(Keys.Space);
            CreateEntity("TitelScrean").SetLocalPosition(Screen.Center).AddComponent(new SpriteRenderer(Content.LoadTexture("ExplosionKaoUwu")));
        }

        public override void Update()
        {
            base.Update();
            if (Button.IsPressed)
            {
                Core.StartSceneTransition(new TextureWipeTransition(() => new Battle(), Content.LoadTexture("nez/textures/textureWipeTransition/pokemon")));
            }
        }
    }
}
