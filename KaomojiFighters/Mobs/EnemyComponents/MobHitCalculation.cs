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
using Nez.Tweens;

namespace KaomojiFighters.Mobs
{
    class MobHitCalculation : Component,  ITelegramReceiver
    {
        public Stats Stats;
        public Entity opponentEntity;
        private List<Attack> EnemyAttacks;
        public BoxCollider HitBox;
        private SpriteRenderer sprite;



        public override void OnAddedToEntity()
        {
 
            base.OnAddedToEntity();
            Stats = Entity.GetComponent<Stats>();
            EnemyAttacks = opponentEntity.GetComponents<Attack>();
            TelegramService.Register(this, Entity.Name);
            sprite = Entity.GetComponent<SpriteRenderer>();
            HitBox = Entity.AddComponent(new BoxCollider(sprite.Width, sprite.Height));
        }

    

        public void MessageReceived(Telegram message)
        {
           if(message.Head == "auf die Fresse")
            {
                        Stats.HP -= opponentEntity.GetComponent<Stats>().AttackValue;
                        System.Diagnostics.Debug.WriteLine(Stats.HP);
                        sprite.Sprite = Stats.sprites.Hurt;
                        var Lammarsch = Math.Sign(Entity.Position.X - opponentEntity.Position.X);
                        if (Stats.HP <= 0) return;
                        Entity.Tween("Position", new Vector2(Entity.Position.X + 200 * Lammarsch, Entity.Position.Y - 25), 0.2f).SetLoops(LoopType.PingPong, 1).SetLoopCompletionHandler((x) => sprite.Sprite = Stats.sprites.Normal).Start();
                        Entity.Tween("Rotation", (float)Lammarsch, 0.2f).SetLoops(LoopType.PingPong, 1).SetFrom(0).Start();
            }
        }
    }
}
