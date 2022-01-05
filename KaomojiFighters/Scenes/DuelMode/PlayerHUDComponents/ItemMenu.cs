using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class ItemMenu : Component, IUpdatable
    {
        public Entity playerEntity;
        private Player player;
        public SpriteRenderer ItemMenuDisplay;
        public List<SpriteRenderer> Textures;
        private BattleHUD hud;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Up;
        private VirtualButton Down;
        private VirtualButton Enter;
        private VirtualButton ExitItemMenu;
        public SpriteRenderer SelectionButton;


        public override void OnAddedToEntity()
        {
            var scene = new Scene();
            base.OnAddedToEntity();

            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Up = new VirtualButton().AddKeyboardKey(Keys.W);
            Down = new VirtualButton().AddKeyboardKey(Keys.S);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            ExitItemMenu = new VirtualButton().AddKeyboardKey(Keys.Back);

            hud = Entity.GetComponent<BattleHUD>();
            player = playerEntity.GetComponent<Player>();

            ItemMenuDisplay = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("ItemMenu")));
            ItemMenuDisplay.LocalOffset = Screen.Center;
            ItemMenuDisplay.Enabled = false;
            ItemMenuDisplay.LayerDepth = 0.1f;

            SelectionButton =
                Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            SelectionButton.LocalOffset =
                new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40, Screen.Center.Y - 30);
            SelectionButton.Enabled = false;

            Textures = new List<SpriteRenderer>();
            for (var i = 0; i < player.ItemList.Count; i++)
            {
                Textures.Add(Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(player.ItemList[i].ItemType))));
                Textures[i].Size = new Vector2(27, 45);
                if (i < 6)
                {
                    Textures[i].LocalOffset = new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * (i + 1), Screen.Center.Y - 30);
                }
                else
                {
                    Textures[i].LocalOffset = new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * (i - 5), Screen.Center.Y + 20);
                }
                Textures[i].RenderLayer = 0;
                Textures[i].LayerDepth = 0;
                Textures[i].Enabled = false;
            }


        }

        public void Update()
        {
            if (hud.Enabled)
            {
                hud.Enabled = false;
            }
            if (SelectionButton.LocalOffset.X - 40 >= Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 && Left.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X - 40, SelectionButton.LocalOffset.Y);
            }
            if (SelectionButton.LocalOffset.X + 40 <= Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * 6 && Right.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X + 40, SelectionButton.LocalOffset.Y);
            }
            if (SelectionButton.LocalOffset.Y - 50 >= Screen.Center.Y - 30 && Up.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y - 50);
            }
            if (SelectionButton.LocalOffset.Y + 50 <= Screen.Center.Y + 20 && Down.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y + 50);
            }

            if (Enter.IsPressed)
            {
                for (var i = 0; i < Textures.Count; i++)
                {
                    if (SelectionButton.LocalOffset == Textures[i].LocalOffset)
                    {
                        player.ItemList[i].ItemEffect();
                        player.ItemList.RemoveAt(i);
                        Textures[i].Enabled = false;
                        Textures.RemoveAt(i);
                    }
                }
            }


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

        private bool PointsAtItem(Vector2 vector2)
        {
            foreach (var itemTexture in Textures)
            {
                if (vector2 == itemTexture.LocalOffset)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
