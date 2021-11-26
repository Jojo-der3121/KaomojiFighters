﻿using System;
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
        public BoxCollider collider;
        private VirtualButton PunchButton;
        SpriteRenderer EntitySprite;
        public string DistinguishedEasterEgg = "";
        

        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            collider = Entity.AddComponent(new BoxCollider(117, -50, 75, 75));
            collider.Enabled = false;
            PunchButton = new VirtualButton(new VirtualButton.MouseLeftButton());
            var frig = Color.CornflowerBlue;
        }

        public void Update()
        {
            if (PunchButton.IsPressed && duration == 0)
            {
                duration = 25;
                collider.Enabled = true;
                EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01Attack" + DistinguishedEasterEgg));
            }

            if (duration == 0) return;
            duration--;
            if (duration != 0) return;
            EntitySprite.Sprite = new Sprite(scene.Content.LoadTexture("Kaomoji01" + DistinguishedEasterEgg));
            collider.Enabled = false;
        }
    }
}
