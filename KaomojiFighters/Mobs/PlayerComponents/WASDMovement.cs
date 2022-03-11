using System;
using System.Collections.Generic;
using System.Text;
using KaomojiFighters.Scenes.OwOWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Mobs
{
    class WASDMovement : Component, IUpdatable, ITriggerListener
    {
        private VirtualJoystick joystick;
        public Mover mover;

        public override void OnAddedToEntity()
        {
            joystick = new VirtualJoystick(true, new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.A, Keys.D, Keys.W, Keys.S));
            mover = Entity.AddComponent(new Mover());
            
        }


        public void Update()
        {
            mover.Move(joystick.Value * 10, out var collisionResult);
        }

        public void OnTriggerEnter(Collider other, Collider local)
        {
            if (other is OwOWorldTrigger owor)
            {
                switch (owor.owoWorldTriggerType)
                {
                    case Enums.OwOWOrldTriggerTypes.battle:
                        Core.StartSceneTransition(new TextureWipeTransition(() => new Battle(), Core.Content.LoadTexture("nez/textures/textureWipeTransition/pokemon")));
                        break;
                }

            }
        }

        public void OnTriggerExit(Collider other, Collider local)
        {
            
        }
    }
}
