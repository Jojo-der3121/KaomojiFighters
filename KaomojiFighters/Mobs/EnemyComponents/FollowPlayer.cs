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

namespace KaomojiFighters.Mobs
{
    class FollowPlayer : Component, IUpdatable
    {

        public float LerpIndex;
        private int buffer = 0;

        public void Update()
        {
            if (Entity.GetComponent<Stats>().ItsMyTurn)
            {
                Entity.Position = Vector2.Lerp(Entity.Position, Screen.Center, LerpIndex);
                if (buffer == 0)
                {
                    buffer = 123;
                }

            }
            if (buffer != 0)
            {
                buffer--;
                if (buffer == 0)
                {
                    Entity.Position = new Vector2(1700, 700);
                    Entity.GetComponent<Stats>().ItsMyTurn = false;
                }
            }
        }
    }
}
