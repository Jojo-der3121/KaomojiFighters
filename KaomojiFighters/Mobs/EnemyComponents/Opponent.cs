using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    class Opponent : Component, IUpdatable
    {
        private Scene scene;
        private Stats stats;
        private Attack attack;
        private Entity opponent;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            opponent = Entity.Scene.FindEntity("Kaomoji01");
            stats = Entity.AddComponent(new Stats() { HP = 49, AttackValue = 2, Speed = 7, sprites = new Enums.Sprites() { Normal = "Kaomoji02", Attack = "Kaomoji02Attack", Hurt = "Kaomoji02Hurt" }, startXPosition = 1400 });
            attack = Entity.AddComponent(new Attack() { attackTarget = opponent });
            attack.Enabled = false;
            Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture(stats.sprites.Normal)));
            Entity.AddComponent(new MobHitCalculation() { opponentEntity = opponent});
        }

        public void Update()
        {
            if (stats.ItsMyTurn)
            {
                attack.Enabled = true;
            }
        }
    }
}
