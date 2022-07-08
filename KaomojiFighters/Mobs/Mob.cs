using KaomojiFighters.Scenes.DuelMode;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    abstract class Mob : Component , ITelegramReceiver
    {
        public Stats stat;
        protected SpriteRenderer spriteRenderer;
        protected Entity opponent;

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

        protected abstract void LoadShit();

        

        public abstract void MessageReceived(Telegram message);
       
    }
}
