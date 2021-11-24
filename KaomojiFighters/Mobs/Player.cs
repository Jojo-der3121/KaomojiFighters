using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace KaomojiFighters.Mobs
{
    class Player : Component
    {
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            Entity.AddComponent(new Punch());
            Entity.AddComponent(new WASDMovement());

        }
    }
}
