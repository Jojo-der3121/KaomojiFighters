using KaomojiFighters.Mobs;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents
{
    class AttackMenu : RenderableComponent
    {
        
        public List<TextComponent> attackList= new List<TextComponent>();
        public Player player;
        private Texture2D TextButton;
        private Texture2D AttackOptionsMenu;
        private VirtualButton Up;
        private VirtualButton Down;
        private VirtualButton Enter;
        private VirtualButton ExitAttackMenu;
        private HUD hud;
        private int selectionY;
        private float selectedElement;
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            hud = Entity.GetComponent<HUD>();
            TextButton = Entity.Scene.Content.LoadTexture("TextButton");
            AttackOptionsMenu= Entity.Scene.Content.LoadTexture("AttackOptions");
            foreach (var item in player.AttackList)
            {
                attackList.Add(Entity.AddComponent(new TextComponent())) ;
            }

            // define Buttons
            Up = new VirtualButton().AddKeyboardKey(Keys.W);
            Down = new VirtualButton().AddKeyboardKey(Keys.S);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            ExitAttackMenu = new VirtualButton().AddKeyboardKey(Keys.Back);

            selectionY = (int)Screen.Center.Y + TextButton.Height + 15;
        }

        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(TextButton,new Vector2(Screen.Center.X-TextButton.Width/2,Screen.Center.Y));
            batcher.Draw(AttackOptionsMenu, new RectangleF(Screen.Center.X - 125, Screen.Center.Y  +TextButton.Height +10 , 250,150));
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 118, selectionY, 236, 25),Color.DarkOliveGreen);
            for (int i = 0; i < player.AttackList.Count; i++)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, player.AttackList[i].attackName,new Vector2(Screen.Center.X- 115, Screen.Center.Y + TextButton.Height + 15 + i*25), Color.Black,0f,Vector2.Zero,2.5f,SpriteEffects.None,0f);
            }
            batcher.DrawString(Graphics.Instance.BitmapFont, player.AttackList[(int)selectedElement / 25].attackName, new Vector2(Screen.Center.X - TextButton.Width/2 +10, Screen.Center.Y + 10), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
        }

        public void Update()
        {
            if (selectionY - 25 >= Screen.Center.Y + TextButton.Height + 15 && Up.IsPressed)
            {
               selectionY -= 25;
                selectedElement = selectionY - (Screen.Center.Y + TextButton.Height + 15);
            }
            if (selectionY + 25 <= Screen.Center.Y + TextButton.Height + 115 && Down.IsPressed)
            {
                selectionY += 25;
                selectedElement = selectionY - (Screen.Center.Y + TextButton.Height + 15);
            }

            // choose selected Attack
            if (Enter.IsPressed)
            {
                player.AttackList[(int)selectedElement / 25].enableAttack();
                Enabled = false;
            }

            // Exit
            if (ExitAttackMenu.IsPressed)
            {       
                Enabled = false;
            }
        }
    }
}
