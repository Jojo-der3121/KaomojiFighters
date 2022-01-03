using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    class Player : Component
    {
        private Scene scene;
        public BoxCollider HitBox;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            Entity.AddComponent(new Punch());
            Entity.AddComponent(new WASDMovement());
            Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji01")));
            //Entity.AddComponent(new Stats() { HP = 35, AttackValue = 3, Speed = 3});
        }
    }
}
