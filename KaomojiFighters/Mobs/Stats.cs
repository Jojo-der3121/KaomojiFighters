using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Enums;
using KaomojiFighters.Scenes.DuelMode;
using Nez;

namespace KaomojiFighters.Mobs
{
    class Stats: Component
    {
        public int HP;
        public int AttackValue;
        public int Speed;
        public bool ItsMyTurn = false;
        public Sprites sprites;
        public int startXPosition;


        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var speedoMeter = Entity.Scene.GetOrCreateSceneComponent<SpeedoMeter>();
            speedoMeter.EntityList.Add(this);
        }
    }
}
