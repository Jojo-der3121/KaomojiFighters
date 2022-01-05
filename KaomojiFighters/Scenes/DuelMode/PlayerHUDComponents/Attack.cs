using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class Attack : Component, IUpdatable
    {
        private Scene scene;
        private int duration;
        public BoxCollider collider;
        public Entity attackTarget;
        SpriteRenderer EntitySprite;
        private Stats stat;
        private bool attackedAlready;
        

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            scene = new Scene();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            stat = Entity.GetComponent<Stats>();
            collider = Entity.AddComponent(new BoxCollider(75, 75));
            collider.Enabled = false;
        }

        private float GetAttackX()
        {
            if (stat.startXPosition < attackTarget.GetComponent<Stats>().startXPosition)
            {
                return Entity.GetComponent<MobHitCalculation>().HitBox.Width/2;
            }
            else
            {
                return -Entity.GetComponent<MobHitCalculation>().HitBox.Width / 2;
            }
        }

        public void Update()
        {
            if (!attackedAlready)
            {
                Entity.Position = Vector2.Lerp(Entity.Position, new Vector2(EnemyXPosition(), attackTarget.Position.Y), 0.06f);  
            }

            if (stat.ItsMyTurn && !attackedAlready && Entity.Position.X <= EnemyXPosition()+5 && Entity.Position.X >= EnemyXPosition() -5)
            {
                duration = 12;
                collider.Enabled = true;
                collider.LocalOffset = new Vector2(GetAttackX(), -50);
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture(stat.sprites.Attack));
                EntitySprite.Size = new Vector2(373, EntitySprite.Height);
                attackedAlready = true;
            }
            if (duration != 0)
            {
                duration--;
                if (duration == 0)
                {
                    EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture(stat.sprites.Normal));
                    EntitySprite.Size = new Vector2(310, EntitySprite.Height);
                    collider.Enabled = false;
                }
            }
            if (attackedAlready && duration == 0)
            {
                Entity.Position = Vector2.Lerp(Entity.Position, new Vector2(stat.startXPosition, 700), 0.04f);
                if (Entity.Position.X >= stat.startXPosition -2 && Entity.Position.X <= stat.startXPosition +2)
                {
                    this.Enabled = false;
                    stat.ItsMyTurn = false;
                    attackedAlready = false;
                }
            }

        }

        private float EnemyXPosition()
        {
            if (stat.startXPosition< attackTarget.GetComponent<Stats>().startXPosition)
            {
                return  attackTarget.Position.X - attackTarget.GetComponent<SpriteRenderer>().Width / 2 - Entity.GetComponent<SpriteRenderer>().Width / 2 - 10;
            }
            else
            {
                return attackTarget.Position.X + attackTarget.GetComponent<SpriteRenderer>().Width / 2 + Entity.GetComponent<SpriteRenderer>().Width / 2 + 10;
            }
        }
    }
}
