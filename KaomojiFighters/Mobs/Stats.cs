using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Enums;
using KaomojiFighters.Scenes.DuelMode;
using Microsoft.Xna.Framework;
using Nez;

namespace KaomojiFighters.Mobs
{
    class Stats: Component
    {
        public int HP;
        public int AttackValue;
        public int Speed;
        public Sprites sprites;

        public override void OnAddedToEntity() => Entity.Scene.GetSceneComponent<SpeedoMeter>()?.EntityList.Add(this);
    }
    
}
