using KaomojiFighters.Enums;
using KaomojiFighters.Scenes.DuelMode;
using KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    abstract class Attack : RenderableComponent, IUpdatable
    {
        protected Scene scene;
        public BoxCollider collider;
        public Entity attackTarget;
        public SpriteRenderer EnemySprite;
        protected SpriteRenderer EntitySprite;
        protected Stats stat;
        protected AttackState oldAttackState;
        protected AttackState attackState;
        protected int Lammarsch;
        private MobHitCalculation MyAutsch;
        protected Vector2 OriginalPosition;
        protected TextComponent attackTxt;
        protected SpriteRenderer Speechbubble;
        protected Texture2D Bubble;
        public string attackName;
        protected int fixedXPosition;
        protected int fixedEnemyXPosition;
        protected float textBuffer;

        public void enableAttack()
        {
            attackState = AttackState.approaching;

        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;


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
            Bubble = Entity.Scene.Content.LoadTexture("SpeachBubble");
        }

        protected float GetAttackX() => -Lammarsch * MyAutsch.HitBox.Width / 2;

        public void Update() => attack();

        protected abstract void attack();

        protected float EnemyXPosition() => attackTarget.Position.X + Lammarsch * (+EnemySprite.Width / 2 + EntitySprite.Width / 2 + 10);

    }

    class s1 : Attack
    {

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            attackName = "s1";
        }
        protected override void attack()
        {
            if (attackState == AttackState.approaching && oldAttackState != AttackState.approaching)
            {
                Entity.Tween("Position", new Vector2(EnemyXPosition(), attackTarget.Position.Y), 0.5f).SetCompletionHandler((x) => attackState = AttackState.attacking).Start();
            }

            if (attackState == AttackState.attacking && oldAttackState != AttackState.attacking)
            {
                collider.Enabled = true;
                collider.LocalOffset = new Vector2(GetAttackX(), -50);
                EntitySprite.SetSprite(new Sprite(stat.sprites.Attack), SpriteRenderer.SizingMode.Resize);
                Core.Schedule(0.21f, (x) => attackState = AttackState.returning);
                TelegramService.SendPrivate(new Telegram(Entity.Name, attackTarget.Name, "auf die Fresse", "tach3tach3tach3"));
            }

            if (attackState == AttackState.returning && oldAttackState != AttackState.returning)
            {
                EntitySprite.SetSprite(new Sprite(stat.sprites.Normal), SpriteRenderer.SizingMode.Resize);
                collider.Enabled = false;

                Entity.Tween("Position", OriginalPosition, 1).SetCompletionHandler((x) =>
                 {
                     if (Entity == null) return;
                     TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                     TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));

                 }
                ).Start();
            }
            oldAttackState = attackState;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {

        }
    }
    class EmotionalDamage : Attack
    {
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            attackName = "get a real Job!!";
        }
        protected override void attack()
        {
            if (attackState == AttackState.approaching && oldAttackState != AttackState.approaching)
            {
                if (Lammarsch == -1)
                {
                    textBuffer = EntitySprite.Width;
                }
                fixedXPosition = (int)(GetAttackX() + textBuffer);
                fixedEnemyXPosition = (int)EnemyXPosition();

                collider.Enabled = true;
                collider.LocalOffset = new Vector2(GetAttackX(), -50);
                EntitySprite.SetSprite(new Sprite(stat.sprites.Attack), SpriteRenderer.SizingMode.Resize);
                TelegramService.SendPrivate(new Telegram(Entity.Name, attackTarget.Name, "auf die Fresse", "tach3tach3tach3"));
                Core.Schedule(0.5f, (x) => attackState = AttackState.attacking);
            }
            if (attackState == AttackState.attacking && oldAttackState != AttackState.attacking)
            {
                collider.Enabled = false;
                EntitySprite.SetSprite(new Sprite(stat.sprites.Normal), SpriteRenderer.SizingMode.Resize);
                Core.Schedule(0.7f, (x) => attackState = AttackState.returning);
            }
            if (attackState == AttackState.returning && oldAttackState != AttackState.returning)
            {
                
                if (Entity == null) return;
                TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));
                Core.Schedule(0.7f, (x) => attackState = AttackState.waiting);
            }
            oldAttackState = attackState;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            if (attackState == AttackState.approaching || attackState == AttackState.attacking || attackState == AttackState.returning)
            {
                batcher.Draw(Bubble, new Rectangle(fixedXPosition -10 , 300-5,180,30));
                batcher.DrawString(Graphics.Instance.BitmapFont, "Get a real Job",new Vector2(fixedXPosition,300), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }
            if(attackState == AttackState.approaching)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, "- Emotional DAMAGE", new Vector2( fixedEnemyXPosition + EnemySprite.Width, attackTarget.LocalPosition.Y), Color.Red, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            }
            if (attackState == AttackState.attacking || attackState == AttackState.returning)
            {
                batcher.Draw(Bubble, new Rectangle(fixedEnemyXPosition-20 + 100, 350 - 5, 500, 30));
                batcher.DrawString(Graphics.Instance.BitmapFont, "beeing a scamer is a real Job, OK ?!", new Vector2(fixedEnemyXPosition + 100, 350), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }
            if (attackState == AttackState.returning )
            {
                batcher.Draw(Bubble, new Rectangle(fixedXPosition -25, 400 - 5, 650, 30));
                batcher.DrawString(Graphics.Instance.BitmapFont, "And im sure you're parents are really proud of you", new Vector2(fixedXPosition, 400), Color.Black,0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }

            // FIX EX POSITION 
        }
    }

    //class Friendzone: Attack
    //{

    //}
}
