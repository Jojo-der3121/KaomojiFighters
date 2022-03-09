using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Mobs
{
    class WASDMovement: Component, IUpdatable
    {
        private VirtualJoystick joystick;

        public override void OnAddedToEntity()=> joystick = new VirtualJoystick(true, new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.A, Keys.D, Keys.W, Keys.S));


        public void Update()
        {
            Entity.Position += joystick.Value*10;
        }
    }
}
