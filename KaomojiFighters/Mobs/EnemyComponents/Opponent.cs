using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class Opponent : Component, ITelegramReceiver
    {
        private Stats stats;
        private Attack attack;
        private Entity opponent;
        public List<word> words;

        public void MessageReceived(Telegram message)
        {
           if( message.Head == "its your turn")
            {
                attack.Enabled = true;
                attack.enableAttack();
            }
            if (message.Head == "its not your turn")
            {
                attack.Enabled = false;
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            TelegramService.Register(this, Entity.Name);
            opponent = Entity.Scene.FindEntity("Kaomoji01"); 
            stats = Entity.AddComponent(new Stats() { HP = 49, AttackValue = 1, Speed = 7, sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Hurt"))) });
            attack = Entity.AddComponent(new EnemyTextAttack() { attackTarget = opponent });
            attack.Enabled = false;
            Entity.AddComponent(new SpriteRenderer(stats.sprites.Normal));
            Entity.AddComponent(new MobHitCalculation() { opponentEntity = opponent});
            words = new List<word>();
            words.Add(Entity.AddComponent(new I()));
            words.Add(Entity.AddComponent(new Hope()));
            words.Add(Entity.AddComponent(new You()));
            words.Add(Entity.AddComponent(new StepOn()));
            words.Add(Entity.AddComponent(new Legos()));

        }

        
    }
}
