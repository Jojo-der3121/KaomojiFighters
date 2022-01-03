using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class Attack : Component
    {
        private SpriteRenderer AttackButton;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var scene = new Scene();
            AttackButton = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("AttackKaoButton")));
            AttackButton.Size = new Vector2(AttackButton.Width * 2.7f, AttackButton.Height * 2.5f);
            AttackButton.RenderLayer = 0;
            AttackButton.LayerDepth = 0.1f;
            AttackButton.LocalOffset = new Vector2(1920 / 4 - 350, 1080 / 2 - 380);
        }
    }
}
