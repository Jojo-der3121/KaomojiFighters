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
        private Vector2 LastFrameMovementSpeed;

        public override void OnAddedToEntity()=> joystick = new VirtualJoystick(true, new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.A, Keys.D, Keys.W, Keys.S));

        public void Update()
        {
            LastFrameMovementSpeed = Vector2.Lerp(LastFrameMovementSpeed, joystick.Value * 13,  0.02f);
            Entity.Position += LastFrameMovementSpeed;
        }
    }
}
