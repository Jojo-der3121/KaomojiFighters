using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

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
            stats = Entity.AddComponent(new Stats() { HP = 35, AttackValue = 7, Speed = 3, sprites = new Enums.Sprites() { Normal = "Kaomoji01", Attack = "Kaomoji01Attack", Hurt = "Kaomoji01Hurt" }, startPosition = new Vector2(600,700)  });
            texture = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture(stats.sprites.Normal)));
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
