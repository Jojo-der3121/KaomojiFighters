using System;
using System.Collections.Generic;
using System.Text;
using KaomojiFighters.Enums;
using KaomojiFighters.Scenes;
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
        VirtualButton interact;
        bool inTrigger;
        private OwOWOrldTriggerTypes colliderTyp;

        public override void OnAddedToEntity()
        {
            joystick = new VirtualJoystick(true, new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.A, Keys.D, Keys.W, Keys.S));
            mover = Entity.AddComponent(new Mover());
            interact = new VirtualButton().AddKeyboardKey(Keys.E);
            Entity.AddComponent(new BoxCollider());

        }


        public void Update()
        {
            mover.Move(joystick.Value * 10, out var collisionResult);
            if (inTrigger&& interact.IsPressed)
            {
                switch (colliderTyp )
                {
                    case OwOWOrldTriggerTypes.Shop:
                        inTrigger = false;
                        colliderTyp = OwOWOrldTriggerTypes.NaNi;
                        Core.StartSceneTransition(new TextureWipeTransition(() => new ShopScene(Entity.LocalPosition), Entity.Scene.Content.LoadTexture("c")));
                        break;
                    //case OwOWOrldTriggerTypes.Dialog:
                    //    Core.StartSceneTransition(new TextureWipeTransition(() => new OverworldScene(), Entity.Scene.Content.LoadTexture("c")));
                    //    break;
                }
            }
        }

        public void OnTriggerEnter(Collider other, Collider local)
        {
            if (other is OwOWorldTrigger owor)
            {
                switch (owor.owoWorldTriggerType)
                {
                    case OwOWOrldTriggerTypes.battle:
                        Core.StartSceneTransition(new TextureWipeTransition(() => new Battle(), Core.Content.LoadTexture("nez/textures/textureWipeTransition/pokemon")));
                        break;
                    case OwOWOrldTriggerTypes.Goal:
                        Core.StartSceneTransition(new TextureWipeTransition(() => new MenuScene(), Core.Content.LoadTexture("a")));
                        break;
                    case OwOWOrldTriggerTypes.Shop:
                        inTrigger = true;
                        colliderTyp= OwOWOrldTriggerTypes.Shop;
                        break;
                        case OwOWOrldTriggerTypes.Dialog:
                        inTrigger = true;
                        colliderTyp = OwOWOrldTriggerTypes.Dialog;
                        break;
                }

            }
        }

        

        public void OnTriggerExit(Collider other, Collider local)
        {
            inTrigger = false;
            colliderTyp = OwOWOrldTriggerTypes.NaNi;
        }
    }
}
