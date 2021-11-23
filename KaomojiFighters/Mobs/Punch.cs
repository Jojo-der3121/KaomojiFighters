using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Rectangle = System.Drawing.Rectangle;

namespace KaomojiFighters.Mobs
{
    class Punch : Component, IUpdatable
    {
        private int duration;
        private Scene scene;
        private BoxCollider collider;
        private VirtualButton PunchButton;
        SpriteRenderer EntitySprite;

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            EntitySprite = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji01")));
            collider = Entity.AddComponent(new BoxCollider(117, -50, 75, 75));
            collider.Enabled = false;
            PunchButton = new VirtualButton(new VirtualButton.MouseLeftButton());
        }

        public void Update()
        {
            if (PunchButton.IsPressed && duration == 0)
            {
                duration =25;
                collider.Enabled  = true;
            }
            if (duration != 0)
            {
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01Attack"));
                duration--;
                if (duration == 0)
                {
                    EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01"));
                    collider.Enabled = false;
                }
            }
        }
    }
}
