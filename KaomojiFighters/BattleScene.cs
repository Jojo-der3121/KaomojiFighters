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

namespace KaomojiFighters
{
    class Battle : Scene
    {
        Texture2D playerSprite;
        private EnemyKaomoji enemyKaomoji;
        SpriteRenderer _player;
        private SpriteRenderer renderer;
        private int _horizontalMovementSpeed = 0;
        private int _vertikalMovementSpeed = 0;
        private EnemyKaomoji _enemy;

        public Battle()
        {
            enemyKaomoji = new EnemyKaomoji();
            renderer = new SpriteRenderer(enemyKaomoji.Sprite);
            playerSprite = Texture2D.FromFile(Core.GraphicsDevice, "C:\\Users\\jbb\\Pictures\\Saved Pictures\\Kaomoji01.png");
            PlayScene();
        }

        private void PlayScene()
        {
            AddRenderer(new DefaultRenderer());
            _player = CreateEntity("Kaomoji01").AddComponent(new SpriteRenderer(playerSprite));
            _player.Transform.LocalPosition = new Vector2(400, 700);
            _enemy = CreateEntity("Kaomoji02").AddComponent(enemyKaomoji);
            _enemy.AddComponent(renderer).LocalOffset = enemyKaomoji.position;
        }

        public override void Update()
        {
            if (Input.CurrentKeyboardState.IsKeyDown(Keys.W))
            {
                _horizontalMovementSpeed--;
            }
            else if (_horizontalMovementSpeed < 0)
            {
                _horizontalMovementSpeed++;
            }
            if (Input.CurrentKeyboardState.IsKeyDown(Keys.S))
            {
                _horizontalMovementSpeed++;
            }
            else if (_horizontalMovementSpeed > 0)
            {
                _horizontalMovementSpeed--;
            }
            if (Input.CurrentKeyboardState.IsKeyDown(Keys.A))
            {
                _vertikalMovementSpeed--;
            }
            else if (_vertikalMovementSpeed < 0)
            {
                _vertikalMovementSpeed++;
            }
            if (Input.CurrentKeyboardState.IsKeyDown(Keys.D))
            {
                _vertikalMovementSpeed++;
            }
            else if (_vertikalMovementSpeed > 0)
            {
                _vertikalMovementSpeed--;
            }
            _player.Transform.LocalPosition =
                new Vector2(_player.Transform.LocalPosition.X + _vertikalMovementSpeed * Time.DeltaTime*7, _player.Transform.LocalPosition.Y + _horizontalMovementSpeed * Time.DeltaTime*7);

            _enemy.Update(_player.Transform.LocalPosition);
            _enemy.Transform.LocalPosition = _enemy.position;
            base.Update();
        }
    }
}
