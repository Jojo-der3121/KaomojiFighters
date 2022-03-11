using System.Collections.Generic;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using KaomojiFighters.Scenes.DuelMode;
using Microsoft.Xna.Framework;

namespace KaomojiFighters.Mobs
{
    class Player : Component, ITelegramReceiver
    {
        private Scene scene;
        private Stats stats;
        public BoxCollider HitBox;
        private SpriteRenderer texture;
        public WASDMovement MovementComponent;
        private EasterEgg easterEgg;
        private bool wasActivatedAlready = false;
        public List<Item> ItemList = new List<Item>();
        

        public override void OnAddedToEntity()
        {
            scene = new Scene(); 
            base.OnAddedToEntity();
            TelegramService.Register(this, Entity.Name);
            stats = Entity.AddComponent(new Stats() { HP = 35, AttackValue = 7, Speed = 15, sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Hurt"))) });
            texture = Entity.AddComponent(new SpriteRenderer(stats.sprites.Normal));
            Entity.AddComponent(new BoxCollider(texture.Width, texture.Height));
            if(Entity.Scene is Battle){
                Entity.AddComponent(new MobHitCalculation() { opponentEntity = Entity.Scene.FindEntity("Kaomoji02") });
            }
            easterEgg = Entity.AddComponent(new EasterEgg() { EasterEggString = new Keys[] { Keys.D, Keys.I, Keys.S, Keys.T, Keys.I, Keys.N, Keys.G, Keys.U, Keys.I, Keys.S, Keys.H, Keys.E, Keys.D } });
            ItemList.Add(Entity.AddComponent( new HealthPotion()));
            ItemList.Add(Entity.AddComponent(new StrenghtPotion()));
            ItemList.Add(Entity.AddComponent(new SpeedPotion()));
            ItemList.Add(Entity.AddComponent( new HealthPotion()));
            ItemList.Add(Entity.AddComponent(new StrenghtPotion()));
            ItemList.Add(Entity.AddComponent(new SpeedPotion()));
            ItemList.Add(Entity.AddComponent( new HealthPotion()));
            ItemList.Add(Entity.AddComponent(new StrenghtPotion()));
            ItemList.Add(Entity.AddComponent(new SpeedPotion()));
        }

        public void MessageReceived(Telegram message)
        { 
            if (message.Head == "Frohe Ostern")
            {
                var oldSize = texture.Size.Y; 
                stats.sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Attack")), new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Hurt"))) ;
                texture.SetSprite(new Sprite(stats.sprites.Normal), SpriteRenderer.SizingMode.Resize);
                texture.LocalOffset = new Vector2(texture.LocalOffset.X, texture.LocalOffset.Y - (texture.Size.Y - oldSize)/2);
                stats.AttackValue *= 50;
                
            }
        }
    }
}
