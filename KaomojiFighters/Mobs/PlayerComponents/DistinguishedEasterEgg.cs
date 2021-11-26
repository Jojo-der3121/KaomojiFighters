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
    class DistinguishedEasterEgg : Component, IUpdatable
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
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        EasterEggCache += "d";
                    }
                    break;
                case 1:
                    if (keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache += "i";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.D))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 2:
                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache += "s";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 3:
                    if (keyboardState.IsKeyDown(Keys.T))
                    {
                        EasterEggCache += "t";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 4:
                    if (keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache += "i";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.T))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 5:
                    if (keyboardState.IsKeyDown(Keys.N))
                    {
                        EasterEggCache += "n";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 6:
                    if (keyboardState.IsKeyDown(Keys.G))
                    {
                        EasterEggCache += "g";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.N))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 7:
                    if (keyboardState.IsKeyDown(Keys.U))
                    {
                        EasterEggCache += "u";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.G))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 8:
                    if (keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache += "i";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.U))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 9:
                    if (keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache += "s";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.I))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 10:
                    if (keyboardState.IsKeyDown(Keys.H))
                    {
                        EasterEggCache += "h";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.S))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 11:
                    if (keyboardState.IsKeyDown(Keys.E))
                    {
                        EasterEggCache += "e";
                    }
                    else if (!keyboardState.IsKeyDown(Keys.H))
                    {
                        EasterEggCache = "";
                    }
                    break;
                case 12:
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        EasterEggCache += "d";
                        EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01" + EasterEggCache));
                        EntitySprite.Size = new Vector2(EntitySprite.Width, EntitySprite.Height );
                        Entity.GetComponent<Stats>().AttackValue *= 5;
                        EntitySprite.LocalOffset = new Vector2(0, -50);
                        punch.DistinguishedEasterEgg = EasterEggCache;
                    }
                    else if (!keyboardState.IsKeyDown(Keys.E))
                    {
                        EasterEggCache = "";
                    }
                    break;
            }
        }
    }
}
