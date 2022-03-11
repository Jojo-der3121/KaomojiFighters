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
        public BoxCollider collider;
        public Entity attackTarget;
        public SpriteRenderer EnemySprite;
        protected SpriteRenderer EntitySprite;
        protected Stats stat;
        protected AttackState oldAttackState;
        protected AttackState attackState;
        private int Lammarsch;
        private MobHitCalculation MyAutsch;
        protected Vector2 OriginalPosition;

        public void enableAttack() => attackState = AttackState.approaching;

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
                Entity.Tween("Position", new Vector2(EnemyXPosition(), attackTarget.Position.Y), 0.5f).SetCompletionHandler((x)=>attackState=AttackState.attacking).Start(); 
            }

            if ( attackState == AttackState.attacking && oldAttackState != AttackState.attacking)
            {
                collider.Enabled = true;
                collider.LocalOffset = new Vector2(GetAttackX(), -50);
                EntitySprite.SetSprite( new Sprite(stat.sprites.Attack), SpriteRenderer.SizingMode.Resize);
                Core.Schedule(0.21f, (x) => attackState = AttackState.returning);
                TelegramService.SendPrivate(new Telegram(Entity.Name,attackTarget.Name, "auf die Fresse", "tach3tach3tach3"));
            }
            
            if (attackState == AttackState.returning && oldAttackState != AttackState.returning)
            {
                EntitySprite.SetSprite( new Sprite(stat.sprites.Normal), SpriteRenderer.SizingMode.Resize);
                collider.Enabled = false;

                Entity.Tween("Position", OriginalPosition,1).SetCompletionHandler((x)=> 
                {
                    if (Entity == null) return;
                    TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                    TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));
                    this.Enabled = false;
                }
                ).Start();
                
            }
            oldAttackState = attackState;
        }
    }
}
