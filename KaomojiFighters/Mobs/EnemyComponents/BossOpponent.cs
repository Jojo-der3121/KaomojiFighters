using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Nez;
using Nez.Textures;

namespace KaomojiFighters.Mobs.EnemyComponents
{
    class BossOpponent: Mob, ITelegramReceiver
    {

        private EnemyTextAttack attack;



        protected override string opponentName => "Kaomoji01";

        protected override Stats statsConfig => new Stats()
        {
            name = "Boss01",
            Gold = 45,
            Speed = 11,
            immunity = "ptsd",
            weakness = "insecure",
            sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji03")),
                new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Attack")),
                new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji03Hurt"))),
            wordList = new List<word>(){new word(Wort.AButtfan), new word(Wort.APuppy), new word(Wort.But), new word(Wort.And), new word(Wort.Insulted), new word(Wort.Killed), new word(Wort.AFishHead), new word(Wort.I), new word(Wort.Sused), new word(Wort.You), new word(Wort.Hope), new word(Wort.Fucked), new word(Wort.AButtfan), new word(Wort.DogFood), new word(Wort.You), new word(Wort.Hope), new word(Wort.Legos), new word(Wort.StepOn)}
        };
        
        protected override void LoadShit() => (attack = Entity.AddComponent(new EnemyTextAttack() { attackTarget = opponent })).Enabled = false;

        public override void MessageReceived(Telegram message)
        {
            switch (message.Head)
            {
                case "its your turn":
                    attack.Enabled = true;
                    attack.enableAttack();
                    break;
                case "its not your turn":
                    attack.Enabled = false;
                    break;
            }
        }
    }
}
