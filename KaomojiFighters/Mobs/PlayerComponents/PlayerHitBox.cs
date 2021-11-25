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
            enemyBoxCollider = opponentEntity.GetComponent<BoxCollider>();
        }

        public void Update()
        {
            if (HitBox.CollidesWith(enemyBoxCollider, out var result))
            {
                stats.HP -= opponentEntity.GetComponent<Stats>().AttackValue;
                Entity.Position = new Vector2(Entity.Position.X,Entity.Position.Y- 20);
            }
            if (stats.HP <= 0)
            {
                Entity.Destroy();
            }
        }
    }
}
