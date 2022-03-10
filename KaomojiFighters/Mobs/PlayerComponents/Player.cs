using System.Collections.Generic;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using KaomojiFighters.Scenes.DuelMode;

namespace KaomojiFighters.Mobs
{
    class Player : Component, IUpdatable
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
            stats = Entity.AddComponent(new Stats() { HP = 35, AttackValue = 7, Speed = 15, sprites = new Enums.Sprites() { Normal = "Kaomoji01", Attack = "Kaomoji01Attack", Hurt = "Kaomoji01Hurt" } });
            texture = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture(stats.sprites.Normal)));
            Entity.AddComponent(new BoxCollider(texture.Width, texture.Height));
            if(Entity.Scene is Battle){
                Entity.AddComponent(new MobHitCalculation() { opponentEntity = Entity.Scene.FindEntity("Kaomoji02") }); // change Battle Kao from Base OWOworld Kao
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

        public void Update()
        {
            
            if (easterEgg.IsActivated && !wasActivatedAlready)
            {
                stats.sprites = new Enums.Sprites() { Normal = "Kaomoji01distinguished", Attack = "Kaomoji01Attackdistinguished", Hurt = "Kaomoji01Hurtdistinguished" };
                texture.Sprite = new Sprite(scene.Content.LoadTexture(stats.sprites.Normal));
                stats.AttackValue *= 50;
                wasActivatedAlready = true;
            }
            
        }
    }
}
