using KaomojiFighters.Objects;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework;

namespace KaomojiFighters.Mobs
{
    class Player : Mob, ITelegramReceiver
    {
        protected override string opponentName => "Kaomoji02";

        protected override Stats statsConfig
        {
            get
            {
                var safeFile = SafeFileLoader.LoadStats();
                safeFile.sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Hurt")));
                return safeFile;
            }
        }


        public override void MessageReceived(Telegram message)
        {
            if (message.Head != "Frohe Ostern") return;
            var oldSize = spriteRenderer.Size.Y;
            stat.sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01distinguished")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Attackdistinguished")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Hurtdistinguished")));
            spriteRenderer.SetSprite(new Sprite(stat.sprites.Normal), SpriteRenderer.SizingMode.Resize);
            spriteRenderer.LocalOffset = new Vector2(spriteRenderer.LocalOffset.X, spriteRenderer.LocalOffset.Y - (spriteRenderer.Size.Y - oldSize) / 2);
            stat.AttackValue *= 50;

            SafeFileLoader.SaveStats(stat);
        }

        protected override void LoadShit() => Entity.AddComponent(new EasterEgg() { EasterEggString = new Keys[] { Keys.D, Keys.I, Keys.S, Keys.T, Keys.I, Keys.N, Keys.G, Keys.U, Keys.I, Keys.S, Keys.H, Keys.E, Keys.D } });

    }
}
