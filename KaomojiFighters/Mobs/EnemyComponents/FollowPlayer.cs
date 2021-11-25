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
        private Entity _player;
        public float LerpIndex;

        public override void OnAddedToEntity() =>_player=  Entity.Scene.FindEntity("Kaomoji01");

        public void Update() => Entity.Position = Vector2.Lerp(Entity.Position, _player.Position, LerpIndex);
    }
}
