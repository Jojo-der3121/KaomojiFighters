using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Scenes.OwOWorld
{
    class OwOWorldTrigger: BoxCollider
    {
        public OwOWorldTrigger(int width, int hight) : base(width, hight) => IsTrigger = true;

        public Enums.OwOWOrldTriggerTypes owoWorldTriggerType;
    }

    class LevelWallCollider : BoxCollider
    {
        public LevelWallCollider(int width, int hight) => IsTrigger = true;
    }
}
