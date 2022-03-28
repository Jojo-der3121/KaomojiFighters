using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Enums;
using KaomojiFighters.Scenes.DuelMode;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class Stats: Component
    {

        [Inspectable]
        public int HP = 42;
        [Inspectable]
        public int AttackValue = 1;
        public int Speed;
        public int energy = 10;
        public (Sprite Normal,Sprite Attack, Sprite Hurt) sprites;
        public int Defence = 1;
        public string weakness;

        public override void OnAddedToEntity() => Entity.Scene.GetSceneComponent<SpeedoMeter>()?.EntityList.Add(this);
    }
    
}
