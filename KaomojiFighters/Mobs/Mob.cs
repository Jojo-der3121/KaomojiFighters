using KaomojiFighters.Scenes.DuelMode;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs
{
    abstract class Mob : Component , ITelegramReceiver
    {
        public Stats stat;
        public SpriteRenderer spriteRenderer;
        public Entity opponent;

        protected abstract string opponentName { get; }
        protected abstract Stats statsConfig { get; }
        

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            stat = statsConfig;
            TelegramService.Register(this, Entity.Name);
            spriteRenderer = Entity.AddComponent(new SpriteRenderer(stat.sprites.Normal));
            spriteRenderer.RenderLayer = 2;
            if (Entity.Scene is Battle)
            {
                opponent = Entity.Scene.FindEntity(opponentName);
                Entity.AddComponent(new MobHitCalculation() { opponentEntity = opponent });
                Entity.Scene.GetSceneComponent<SpeedoMeter>().EntityList.Add(this);
            }
            LoadShit();
        }

        public abstract void LoadShit();

        

        public abstract void MessageReceived(Telegram message);
       
    }
}
