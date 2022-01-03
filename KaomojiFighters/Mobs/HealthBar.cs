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
        public PrototypeSpriteRenderer redRectangle;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            _stats = entity.GetComponent<Stats>();
            ProcentPerHP = 700 / _stats.HP;
            HealthMeter = Entity.AddComponent(new PrototypeSpriteRenderer(700, 55) { Color = Color.Green });
            HealthMeter.RenderLayer = 1;
            HealthMeter.LayerDepth = 0;
            redRectangle = Entity.AddComponent(new PrototypeSpriteRenderer(_stats.HP * ProcentPerHP, 55) { Color = Color.Red });
            redRectangle.RenderLayer = 1;
            redRectangle.LayerDepth = 0.1f;
        }

        public void Update()
        {
            HealthMeter.SetWidth(_stats.HP * ProcentPerHP);
        }
    }
}
