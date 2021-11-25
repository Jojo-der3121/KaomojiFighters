using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    class Player : Component
    {
        private Scene scene;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            Entity.AddComponent(new Punch());
            Entity.AddComponent(new WASDMovement());
            Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji01")));
            Entity.AddComponent(new DistinguishedEasterEgg());
            Entity.AddComponent(new Stats() {HP = 30, AttackValue = 3});
        }
    }
}
