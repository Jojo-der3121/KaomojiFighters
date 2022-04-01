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
    class Opponent : Mob, ITelegramReceiver
    {

        private EnemyTextAttack attack;



        protected override string opponentName => "Kaomoji01";

        protected override Stats statsConfig => new Stats() { Speed = 7, weakness = "Mom Jokes", sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Hurt"))), wordList = word.GetWordList() };

        public override void LoadShit() => (attack = Entity.AddComponent(new EnemyTextAttack() { attackTarget = opponent })).Enabled = false;

        public override void MessageReceived(Telegram message)
        {
            if (message.Head == "its your turn")
            {
                attack.Enabled = true;
                attack.enableAttack();
            }
            if (message.Head == "its not your turn")
            {
                attack.Enabled = false;
            }
        }
    }
}
