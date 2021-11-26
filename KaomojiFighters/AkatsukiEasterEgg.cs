using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters
{
    class AkatsukiEasterEgg : Component, IUpdatable
    {
        private string EasterEggCache = "";
        private Scene scene;
        private Punch punch;
        private SpriteRenderer EntitySprite;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            scene = new Scene();
            punch = Entity.GetComponent<Punch>();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.GetPressedKeyCount() != 1) return;
            switch (EasterEggCache.Length)
            {
                case 0:
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        EasterEggCache += "A";
                    }
                    break;
                case 1:
                    if (keyboardState.IsKeyDown(Keys.K))
                    {
                        EasterEggCache += "k";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.A))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 2:
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        EasterEggCache += "a";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.K))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 3:
                    if (keyboardState.IsKeyDown(Keys.T))
                    {
                        EasterEggCache += "t";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.A))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 4:
                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache += "s";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.T))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 5:
                    if (keyboardState.IsKeyDown(Keys.U))
                    {
                        EasterEggCache += "u";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 6:
                    if (keyboardState.IsKeyDown(Keys.K))
                    {
                        EasterEggCache += "k";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.U))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 7:
                    if (keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache += "i";
                        EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("ArenaBackgroundClouds" + EasterEggCache));

                    }
                    else if (!keyboardState.IsKeyDown(Keys.K))
                    {
                        EasterEggCache = "";
                    }
                    break;
            }
        }
    }
}
