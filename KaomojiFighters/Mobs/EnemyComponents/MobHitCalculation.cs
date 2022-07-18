using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Tweens;

namespace KaomojiFighters.Mobs
{
    class MobHitCalculation : RenderableComponent, ITelegramReceiver, IUpdatable
    {
        private Mob mob;
        public Entity opponentEntity;
        public BoxCollider HitBox;
        private SpriteRenderer sprite;
        private int lastTurnHP;
        private int triggerBuffer;
        private int immunityBuffer;



        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            mob = Entity.GetComponent<Mob>();
            TelegramService.Register(this, Entity.Name);
            sprite = Entity.GetComponent<SpriteRenderer>();
            HitBox = Entity.AddComponent(new BoxCollider(sprite.Width, sprite.Height));
        }



        public void MessageReceived(Telegram message)
        {
            if (message.Head != "auf die Fresse") return;
            var substrings = System.Text.RegularExpressions.Regex.Split(message.Body, " ");
            foreach (var topic in substrings)
            {
                if (topic == mob.stat.weakness && triggerBuffer== 0)triggerBuffer = 60;
                if (topic == mob.stat.immunity && immunityBuffer == 0) immunityBuffer = 60;
            }
            if (immunityBuffer > 0 && triggerBuffer > 0)
            {
                immunityBuffer = 0;
                triggerBuffer = 0;
            }
            var damage = triggerBuffer>0 ? opponentEntity.GetComponent<Mob>().stat.AttackValue*2 - mob.stat.Defence: opponentEntity.GetComponent<Mob>().stat.AttackValue - mob.stat.Defence;
            if (immunityBuffer > 0) damage = 0;
            if (damage > 0)
            {
                mob.stat.HP -= damage;
            }
        }

        public void Update()
        {
            if (lastTurnHP > mob.stat.HP)
            {
               sprite.Sprite = mob.stat.sprites.Hurt; 
                var Lammarsch = Math.Sign(Entity.Position.X - opponentEntity.Position.X);
                if (mob.stat.HP <= 0) return;
                Entity.Tween("Position", new Vector2(Entity.Position.X + 200 * Lammarsch, Entity.Position.Y - 25), 0.2f).SetLoops(LoopType.PingPong, 1).SetLoopCompletionHandler((x) => sprite.Sprite = mob.stat.sprites.Normal).Start();
                Entity.Tween("Rotation", (float)Lammarsch, 0.2f).SetLoops(LoopType.PingPong, 1).SetFrom(0).Start();
            }

            if (triggerBuffer > 0) triggerBuffer--;
            if (immunityBuffer > 0) immunityBuffer--;

            lastTurnHP = mob.stat.HP;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            if(triggerBuffer > 0) batcher.DrawString(Graphics.Instance.BitmapFont, "-- Emotional Damage", new Vector2( Math.Sign(Entity.Position.X - opponentEntity.Position.X) > 0? 1560-triggerBuffer: 640+triggerBuffer, 500- triggerBuffer), Color.Red, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            if(immunityBuffer > 0) batcher.DrawString(Graphics.Instance.BitmapFont, "Immunity", new Vector2( Math.Sign(Entity.Position.X - opponentEntity.Position.X) > 0? 1560-immunityBuffer: 640+ immunityBuffer, 500- immunityBuffer), Color.LightGray, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;
    }
}
