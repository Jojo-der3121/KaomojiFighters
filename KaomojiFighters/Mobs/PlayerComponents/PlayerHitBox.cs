using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace KaomojiFighters.Mobs
{
    class PlayerHitBox : Component, IUpdatable
    {
        private Stats stats;
        private Entity opponentEntity;
        private BoxCollider enemyBoxCollider;
        private BoxCollider HitBox;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            stats = Entity.GetComponent<Stats>();
            HitBox = Entity.AddComponent(new BoxCollider());
            opponentEntity = Entity.Scene.FindEntity("Kaomoji02");
        }

        public void Update()
        {
            if (HitBox.CollidesWithAny(out var entityWasAttacked) && entityWasAttacked.Collider == opponentEntity.GetComponent<BoxCollider>())
            {
                stats.HP -= opponentEntity.GetComponent<Stats>().AttackValue;
                Entity.Position = new Vector2(Entity.Position.X - 100, Entity.Position.Y );
            }
            if (stats.HP <= 0)
            {
                Entity.Destroy();
            }
        }
    }
}
