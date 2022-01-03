using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents
{
    class BattleHUD : Component, IUpdatable
    {
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private SpriteRenderer selectionButton;
        private int selectionDestination;
        public int selectedButton = 0;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            var scene = new Scene();
            Entity.AddComponent(new Attack());
            Entity.AddComponent(new ItemMenu());
            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            selectionButton = Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(selectionButton.Width * 3, selectionButton.Height * 3);
            selectionButton.RenderLayer = 0;
            selectionButton.LayerDepth = 0;
            selectionButton.LocalOffset = new Vector2(1920 / 4-350, 1080 / 2 - 375);
            selectionDestination = 1920 / 4 - 350;
        }

        public void Update()
        {

            if (selectionButton.LocalOffset.X - 350 >= 1920 / 4 - 350 && Left.IsPressed)
            {
                selectionDestination -= 350;
            }
            if (selectionButton.LocalOffset.X + 350 <= 1920 / 4 + 350 && Right.IsPressed)
            {
                selectionDestination += 350;
            }
           selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset,new Vector2( selectionDestination, selectionButton.LocalOffset.Y), 0.06f);
            if (Enter.IsPressed)
            {
                selectedButton = selectionDestination;
                if (selectedButton == 1920 / 4 + 350)
                {
                    Entity.GetComponent<Stats>().HP = 0;
                }
            }

        }
    }
}
