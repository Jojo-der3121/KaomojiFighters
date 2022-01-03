using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    class Opponent : Component
    {
        private Scene scene;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            Entity.AddComponent(new Stats() { HP = 49, AttackValue = 2, Speed = 7 });
            Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji02")));
            Entity.AddComponent(new FollowPlayer() { LerpIndex = 0.02f });
            Entity.AddComponent(new MobHitCalculation(){spriteAssetName = "Kaomoji02"});
        }
    }
}
