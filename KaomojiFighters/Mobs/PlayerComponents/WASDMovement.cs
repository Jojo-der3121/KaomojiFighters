using System;
using System.Collections.Generic;
using System.Text;
using KaomojiFighters.Enums;
using KaomojiFighters.Scenes;
using KaomojiFighters.Scenes.OwOWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    class OwOWorldPlayer : Component, IUpdatable, ITriggerListener
    {
        public VirtualJoystick joystick;
        public Stats stat;
        private Mover mover;
        VirtualButton interact;
        bool inTrigger;
        private OwOWOrldTriggerTypes colliderTyp;
        private Vector2 NPCposition;
        public SpriteRenderer renderer;

        public override void OnAddedToEntity()
        {
            stat = SafeFileLoader.LoadStats();
            stat.sprites = (new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01")),
                new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Attack")),
                new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji01Hurt")));
            renderer = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("Kaomoji01")));
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
                        Core.StartSceneTransition(new TextureWipeTransition(() => new ShopScene(), Entity.Scene.Content.LoadTexture("c")));
                        break;
                    case OwOWOrldTriggerTypes.Dialog:
                        inTrigger = false;
                        colliderTyp = OwOWOrldTriggerTypes.NaNi;
                        var OwOworld =(OverworldScene) Entity.Scene;
                        OwOworld.dialogNPCs[NPCposition].Enabled = true;
                        NPCposition = Vector2.Zero;
                        break;
                }
            }
        }

        public void OnTriggerEnter(Collider other, Collider local)
        {
            SafeFileLoader.SaveStats(stat);
            if (!(other is OwOWorldTrigger owor)) return;
            switch (owor.owoWorldTriggerType)
            {
                case OwOWOrldTriggerTypes.battle:
                    Core.StartSceneTransition(new TextureWipeTransition(() => new Battle(), Core.Content.LoadTexture("nez/textures/textureWipeTransition/pokemon")));
                    break;
                case OwOWOrldTriggerTypes.Goal:
                    Core.StartSceneTransition(new TextureWipeTransition(() => new MenuScene(), Core.Content.LoadTexture("a")));
                    break;
                case OwOWOrldTriggerTypes.Shop:
                    stat.OwOworldPosition = Entity.Position;
                    SafeFileLoader.SaveStats(stat);
                    inTrigger = true;
                    colliderTyp= OwOWOrldTriggerTypes.Shop;
                    break;
                case OwOWOrldTriggerTypes.Dialog:
                    inTrigger = true;
                    colliderTyp = OwOWOrldTriggerTypes.Dialog;
                    NPCposition = other.AbsolutePosition;
                    break;
                case OwOWOrldTriggerTypes.LocationSafer:
                    stat.OwOworldPosition = Entity.Position;
                    SafeFileLoader.SaveStats(stat);
                    inTrigger = true;
                    colliderTyp = OwOWOrldTriggerTypes.LocationSafer;
                    break;

            }
        }

        

        public void OnTriggerExit(Collider other, Collider local)
        {
            inTrigger = false;
            colliderTyp = OwOWOrldTriggerTypes.NaNi;
        }
    }
}
