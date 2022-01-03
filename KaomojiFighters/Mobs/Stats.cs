using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace KaomojiFighters.Mobs
{
    class Stats: Component
    {
        public int HP;
        public int AttackValue;
        public int Speed;
        public bool ItsMyTurn = false;
    }
}
