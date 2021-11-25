using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class MobHitCalculation : Component, IUpdatable
    {
        public Stats Stats;
        private Scene scene;
        public int StunTimer;
        private Entity opponentEntity;
        private Punch EnemyAttack;
        private BoxCollider HitBox;
        private SpriteRenderer sprite;
        public string spriteAssetName;
        

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            Stats = Entity.GetComponent<Stats>();
            opponentEntity = Entity.Scene.FindEntity("Kaomoji01");
            EnemyAttack = opponentEntity.GetComponent<Punch>();
            sprite = Entity.GetComponent<SpriteRenderer>();
            HitBox = Entity.AddComponent(new BoxCollider());
        }

        public void Update()
        {
            if (EnemyAttack.collider != null)
            {
                if (HitBox.CollidesWith(EnemyAttack.collider, out var hitResult) && EnemyAttack.collider.Enabled)
                {
                    Stats.HP -= opponentEntity.GetComponent<Stats>().AttackValue;
                    Entity.Position = new Vector2(Entity.Position.X + 200, Entity.Position.Y - 25);
                    Entity.GetComponent<FollowPlayer>().Enabled = false;
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture(spriteAssetName+"Hurt"));
                    StunTimer = 15;
                    Entity.Rotation +=1 ;
                }
            }

            if (StunTimer > 0)
            {
                StunTimer--;
                if (StunTimer == 0)
                {
                    Entity.GetComponent<FollowPlayer>().Enabled =true;
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture(spriteAssetName));
                    Entity.Rotation = 0;
                }
            }
            if (Stats.HP <= 0)
            {
                Entity.Destroy();
            }
        }
    }
}
