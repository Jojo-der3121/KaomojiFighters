using System;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Tweens;

namespace KaomojiFighters.Mobs
{
    class MobHitCalculation : Component, ITelegramReceiver, IUpdatable
    {
        private Stats Stats;
        public Entity opponentEntity;
        public BoxCollider HitBox;
        private SpriteRenderer sprite;
        private int lastTurnHP;



        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            Stats = Entity.GetComponent<Mob>().stat;
            TelegramService.Register(this, Entity.Name);
            sprite = Entity.GetComponent<SpriteRenderer>();
            HitBox = Entity.AddComponent(new BoxCollider(sprite.Width, sprite.Height));
        }



        public void MessageReceived(Telegram message)
        {
            if (message.Head != "auf die Fresse") return;
            var damage = opponentEntity.GetComponent<Mob>().stat.AttackValue - Stats.Defence;
            if (damage > 0)
            {
                Stats.HP -= damage;
            }
        }

        public void Update()
        {
            if (lastTurnHP > Stats.HP)
            {
               sprite.Sprite = Stats.sprites.Hurt; 
                var Lammarsch = Math.Sign(Entity.Position.X - opponentEntity.Position.X);
                if (Stats.HP <= 0) return;
                Entity.Tween("Position", new Vector2(Entity.Position.X + 200 * Lammarsch, Entity.Position.Y - 25), 0.2f).SetLoops(LoopType.PingPong, 1).SetLoopCompletionHandler((x) => sprite.Sprite = Stats.sprites.Normal).Start();
                Entity.Tween("Rotation", (float)Lammarsch, 0.2f).SetLoops(LoopType.PingPong, 1).SetFrom(0).Start();
            }

            lastTurnHP = Stats.HP;
        }
    }
}
