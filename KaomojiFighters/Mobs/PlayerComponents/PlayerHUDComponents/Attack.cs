using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class Attack : Component, IUpdatable
    {
        private Scene scene;
        private int duration;
        public BoxCollider collider;
        public Entity attackTarget;
        SpriteRenderer EntitySprite;
        private Stats stat;
        private bool attackedAlready;
        private float attackTargetSize;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            scene = new Scene();
            collider = Entity.AddComponent(new BoxCollider(117, -50, 75, 75));
            collider.Enabled = false;
            attackTarget = Entity.Scene.FindEntity("Kaomoji02");
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            stat = Entity.GetComponent<Stats>();
        }

        public void Update()
        {
            if (!attackedAlready)
            {
                Entity.Position = Vector2.Lerp(Entity.Position, new Vector2 (attackTarget.Position.X - attackTarget.GetComponent<SpriteRenderer>().Width / 2 - Entity.GetComponent<SpriteRenderer>().Width/2 - 10, attackTarget.Position.Y), 0.06f);
            }

            if (stat.ItsMyTurn && attackedAlready== false && Entity.Position.X <= attackTarget.Position.X - attackTarget.GetComponent<SpriteRenderer>().Width / 2 - Entity.GetComponent<SpriteRenderer>().Width / 2 - 5 && Entity.Position.X >= attackTarget.Position.X - attackTarget.GetComponent<SpriteRenderer>().Width / 2 - Entity.GetComponent<SpriteRenderer>().Width / 2 - 15)
            {
                duration = 12;
                collider.Enabled = true;
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01Attack"));
                EntitySprite.Size = new Vector2(373, EntitySprite.Height);
                attackedAlready = true;
            }
            if (duration != 0)
            {
                duration--;
                if (duration == 0)
                {
                    EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01"));
                    EntitySprite.Size = new Vector2(310, EntitySprite.Height);
                    collider.Enabled = false;
                }
            }
            if (attackedAlready && duration == 0)
            {
                Entity.Position = Vector2.Lerp(Entity.Position, new Vector2(600, 700), 0.04f);
                if (Entity.Position.X >= 598 && Entity.Position.X <= 602)
                {
                    this.Enabled = false;
                    stat.ItsMyTurn = false;
                    attackedAlready = false;
                }
            }

        }
    }
}
