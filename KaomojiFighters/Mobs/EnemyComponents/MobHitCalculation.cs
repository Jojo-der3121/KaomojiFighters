using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
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
        public Entity opponentEntity;
        private Attack EnemyAttack;
        public BoxCollider HitBox;
        private SpriteRenderer sprite;



        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            Stats = Entity.GetComponent<Stats>();
            EnemyAttack = opponentEntity.GetComponent<Attack>();
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
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture(Stats.sprites.Hurt));
                    StunTimer = 25;
                    if (Stats.startPosition.X > opponentEntity.GetComponent<Stats>().startPosition.X)
                    {
                        Entity.Position = new Vector2(Entity.Position.X + 200, Entity.Position.Y - 25);
                        Entity.Rotation += 1;
                    }
                    else
                    {
                        Entity.Position = new Vector2(Entity.Position.X - 200, Entity.Position.Y - 25);
                        Entity.Rotation -= 1;
                    }

                }
            }

            if (StunTimer > 0)
            {
                StunTimer--;
                if (StunTimer == 0)
                {
                    sprite.Sprite = new Sprite(scene.Content.LoadTexture(Stats.sprites.Normal));
                    Entity.Position = Stats.startPosition;
                    Entity.Rotation = 0;
                }
            }
        }
    }
}
