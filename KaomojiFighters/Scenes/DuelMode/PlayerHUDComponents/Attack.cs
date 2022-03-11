using KaomojiFighters.Enums;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    abstract class Attack : Component, IUpdatable
    {
        protected Scene scene;
        protected int duration;
        public BoxCollider collider;
        public Entity attackTarget;
        public SpriteRenderer EnemySprite;
        protected SpriteRenderer EntitySprite;
        protected Stats stat;
        protected AttackState oldAttackState = AttackState.waiting;
        protected AttackState attackState = AttackState.waiting;
        private int Lammarsch;
        private MobHitCalculation MyAutsch;
        protected bool tweenStartet;
        protected Vector2 OriginalPosition;

        public void enableAttack()
        {
            if (attackState != AttackState.waiting) return;
            attackState = AttackState.approaching;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            scene = new Scene();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            stat = Entity.GetComponent<Stats>();
            collider = Entity.AddComponent(new BoxCollider(75, 75));
            collider.Enabled = false;
            EnemySprite = attackTarget.GetComponent<SpriteRenderer>();
            Lammarsch = Math.Sign(Entity.Position.X - attackTarget.Position.X);
            MyAutsch = Entity.GetComponent<MobHitCalculation>();
            OriginalPosition = Entity.Position;
        }

        protected float GetAttackX() => -Lammarsch*MyAutsch.HitBox.Width/2;

        public void Update() => attack();

        protected abstract void attack();
        
        protected float EnemyXPosition() => attackTarget.Position.X + Lammarsch * (+EnemySprite.Width / 2 + EntitySprite.Width / 2 + 10);
       
    }

    class s1 : Attack
    {
        
        
        protected override void attack()
        {
            if (attackState == AttackState.approaching && oldAttackState != AttackState.approaching)
            {
                Entity.Tween("Position", new Vector2(EnemyXPosition(), attackTarget.Position.Y), 0.5f).SetEaseType(Nez.Tweens.EaseType.CubicIn).SetCompletionHandler((x)=>attackState=AttackState.attacking).Start();
                Entity.Position = Vector2.Lerp(Entity.Position, new Vector2(EnemyXPosition(), attackTarget.Position.Y), 0.06f);
               
            }

            if ( attackState == AttackState.attacking && oldAttackState != AttackState.attacking)
            {
                collider.Enabled = true;
                collider.LocalOffset = new Vector2(GetAttackX(), -50);
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture(stat.sprites.Attack));
                EntitySprite.Size = new Vector2(373, EntitySprite.Height);
                Core.Schedule(0.21f, (x) => attackState = AttackState.returning);
                TelegramService.SendPrivate(new Telegram(Entity.Name,attackTarget.Name, "auf die Fresse", "tach3tach3tach3"));
            }
            
            if (attackState == AttackState.returning && oldAttackState != AttackState.returning)
            {
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture(stat.sprites.Normal));
                EntitySprite.Size = new Vector2(310, EntitySprite.Height);
                collider.Enabled = false;

                Entity.Tween("Position", OriginalPosition,1).SetCompletionHandler((x)=> 
                {
                    if (Entity == null) return;
                    TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                    TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));
                    this.Enabled = false;
                    attackState = AttackState.waiting;
                }
                ).Start();
                
            }
            oldAttackState = attackState;
        }
    }
}
