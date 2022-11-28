using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Nez;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class Opponent : Mob, ITelegramReceiver
    {

        private EnemyTextAttack attack;



        protected override string opponentName => "Kaomoji01";

        protected override Stats statsConfig => new Stats() { Gold= 15, Speed = 7,immunity = "insecure", weakness = "MomJokes", sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Hurt"))), wordList = word.GetWordList(), name = "Kaomoji02"};

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
