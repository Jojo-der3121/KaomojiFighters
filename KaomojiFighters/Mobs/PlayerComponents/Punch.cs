using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class Punch : Component, IUpdatable
    {
        private int duration;
        private Scene scene;
        private bool IsSpriteAlreadyChenged;
        public BoxCollider collider;
        SpriteRenderer EntitySprite;
        public EasterEgg easterEgg;
        private Stats stat;


        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            collider = Entity.AddComponent(new BoxCollider(117, -50, 75, 75));
            stat = Entity.GetComponent<Stats>();
            easterEgg = Entity.AddComponent(new EasterEgg() { EasterEggString = new Keys[] { Keys.D, Keys.I, Keys.S, Keys.T, Keys.I, Keys.N, Keys.G, Keys.U, Keys.I, Keys.S, Keys.H, Keys.E, Keys.D } });
            collider.Enabled = false;
            
        }

        public void Update()
        {
            if (!IsSpriteAlreadyChenged && easterEgg.IsActivated)
            {
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01distinguished"));
                EntitySprite.Size = new Vector2(EntitySprite.Width, EntitySprite.Height * 2);
                Entity.GetComponent<Stats>().AttackValue *= 5;
                EntitySprite.LocalOffset = new Vector2(0, -50);
                IsSpriteAlreadyChenged = true;
            }

            if (stat.ItsMyTurn && duration == 0)
            {
                duration = 25;
                collider.Enabled = true;
                if (easterEgg.IsActivated)
                {
                    EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01Attackdistinguished"));
                    EntitySprite.Size = new Vector2(373 , EntitySprite.Height );
                }
                else
                {
                    EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01Attack"));
                    EntitySprite.Size = new Vector2(373, EntitySprite.Height);
                }

            }

            if (duration == 0) return;
            duration--;
            if (duration != 0) return;
            if (easterEgg.IsActivated)
            {
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01distinguished"));
                EntitySprite.Size = new Vector2(310 , EntitySprite.Height);
            }
            else
            {
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01"));
                EntitySprite.Size = new Vector2(310 , EntitySprite.Height);               
            }
            stat.ItsMyTurn = false;
            collider.Enabled = false;

        }
    }
}
