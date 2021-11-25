using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Rectangle = System.Drawing.Rectangle;

namespace KaomojiFighters.Mobs
{
    class HealthBar : Component, IUpdatable
    {
        public Entity entity;
        private Stats _stats;
        private PrototypeSpriteRenderer HealthMeter;
        private int ProcentPerHP;
        private PrototypeSpriteRenderer redRectangle;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            _stats = entity.GetComponent<Stats>();
            HealthMeter = Entity.AddComponent(new PrototypeSpriteRenderer(900, 75) { Color = Color.Green });
            HealthMeter.RenderLayer = 1;
            HealthMeter.LayerDepth = 0;
            ProcentPerHP = 900 / _stats.HP;
            redRectangle = Entity.AddComponent(new PrototypeSpriteRenderer(900, 75) { Color = Color.Red });
            redRectangle.RenderLayer = 1;
            redRectangle.LayerDepth = 1;
        }

        public void Update()
        {
            HealthMeter.SetWidth(_stats.HP * ProcentPerHP);
        }
    }
}
