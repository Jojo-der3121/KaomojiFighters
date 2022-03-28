using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using KaomojiFighters.Scenes.DuelMode;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class ItemMenu : Component
    {
        public Entity playerEntity;
        private Player player;
        public SpriteRenderer ItemMenuDisplay;
        public List<SpriteRenderer> Textures;
        private HUD hud;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Up;
        private VirtualButton Down;
        private VirtualButton Enter;
        private VirtualButton ExitItemMenu;
        public SpriteRenderer SelectionButton;
        private Stats stats;


        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            // assigne Buttons
            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Up = new VirtualButton().AddKeyboardKey(Keys.W);
            Down = new VirtualButton().AddKeyboardKey(Keys.S);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            ExitItemMenu = new VirtualButton().AddKeyboardKey(Keys.Back);

            // get Entities
            hud = Entity.GetComponent<HUD>();
            player = playerEntity.GetComponent<Player>();
            stats = player.Entity.GetComponent<Stats>();

            // ItemDisplay Config
            ItemMenuDisplay = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("ItemMenu")));
            ItemMenuDisplay.LocalOffset = new Vector2(Screen.Center.X - Entity.Transform.Position.X, Screen.Center.Y - Entity.Transform.Position.Y); ;
            ItemMenuDisplay.Enabled = false;
            ItemMenuDisplay.LayerDepth = 0.1f;

            // selecButton config
            SelectionButton =
                Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            SelectionButton.LocalOffset =
                new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 - Entity.Transform.Position.X, Screen.Center.Y - 22 - Entity.Transform.Position.Y);
            SelectionButton.Size = new Vector2(29, 55);
            SelectionButton.Enabled = false;

            // adds ItemList to list of Spriterenderers
            Textures = new List<SpriteRenderer>();
            for (var i = 0; i < player.ItemList.Count; i++)
            {
                Textures.Add(Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(player.ItemList[i].ItemType))));
                Textures[i].Size = new Vector2(27, 45);
                if (i < 6)
                {
                    Textures[i].LocalOffset = new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * (i + 1) - Entity.Transform.Position.X, Screen.Center.Y - 30 - Entity.Transform.Position.Y);
                }
                else
                {
                    Textures[i].LocalOffset = new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * (i - 5) - Entity.Transform.Position.X, Screen.Center.Y+ 20 - Entity.Transform.Position.Y);
                }
                Textures[i].RenderLayer = 0;
                Textures[i].LayerDepth = 0;
                Textures[i].Enabled = false;
            }
            


        }

        public void Update()
        {
            //move cursor if not in boarder
            if (SelectionButton.LocalOffset.X - 40 >= Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 - Entity.Transform.Position.X && Left.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X - 40, SelectionButton.LocalOffset.Y);
            }
            if (SelectionButton.LocalOffset.X + 40 <= Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * 6 - Entity.Transform.Position.X && Right.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X + 40, SelectionButton.LocalOffset.Y);
            }
            if (SelectionButton.LocalOffset.Y - 50 >= Screen.Center.Y - 30 - Entity.Transform.Position.Y && Up.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y - 50);
            }
            if (SelectionButton.LocalOffset.Y + 50 <= Screen.Center.Y + 28 - Entity.Transform.Position.Y && Down.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y + 50);
            }

            // choose selected item
            if (Enter.IsPressed)
            {
                for (var i = 0; i < Textures.Count; i++)
                {
                    if (SelectionButton.LocalOffset.X == Textures[i].LocalOffset.X && SelectionButton.LocalOffset.Y-8 == Textures[i].LocalOffset.Y && stats.energy - player.ItemList[i].cost >= 0)
                    {
                        player.ItemList[i].ItemEffect();
                        player.ItemList.RemoveAt(i);
                        Textures[i].Enabled = false;
                        Textures.RemoveAt(i);
                        break;
                    }
                }
            }

            // Exit
            if (ExitItemMenu.IsPressed)
            {
                ItemMenuDisplay.Enabled = false;
                SelectionButton.Enabled = false;
                foreach (var element in Textures)
                {
                    element.Enabled = false;
                }
                hud.Enabled = true;
                Enabled = false;
            }

        }

    }
}
