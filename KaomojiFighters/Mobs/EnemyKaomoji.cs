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
    class EnemyKaomoji : Component, IUpdatable
    {
        public Texture2D Sprite;
        public Vector2 position = new Vector2(1500, 700);
        public int _eHorizontalMovementSpeed = 0;
        public int _eVertikalMovementSpeed = 0;

        public EnemyKaomoji()
        {
            Sprite = Texture2D.FromFile(Core.GraphicsDevice, "C:\\Users\\jbb\\Pictures\\Saved Pictures\\Kaomoji02.png");
        }

        public void Update()
        {
            if (enemyLocation.X > position.X && _eHorizontalMovementSpeed < 100)
            {
                _eHorizontalMovementSpeed++;
            }
            else if (_eHorizontalMovementSpeed > 0)
            {
                _eHorizontalMovementSpeed--;
            }
            if (enemyLocation.X < position.X && _eHorizontalMovementSpeed > -100)
            {
                _eHorizontalMovementSpeed--;
            }
            else if (_eHorizontalMovementSpeed < 0)
            {
                _eHorizontalMovementSpeed++;
            }
            if (enemyLocation.Y > position.Y && _eVertikalMovementSpeed < 100)
            {
                _eVertikalMovementSpeed++;
            }
            else if (_eVertikalMovementSpeed > 0)
            {
                _eVertikalMovementSpeed--;
            }
            if (enemyLocation.Y < position.Y && _eVertikalMovementSpeed > -100)
            {
                _eVertikalMovementSpeed--;
            }
            else if (_eVertikalMovementSpeed < 0)
            {
                _eVertikalMovementSpeed++;
            }
            position.X += _eHorizontalMovementSpeed * Time.DeltaTime;
            position.Y += _eVertikalMovementSpeed * Time.DeltaTime;
        }


    }
}
