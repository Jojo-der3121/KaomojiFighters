using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using KaomojiFighters.Scenes.DuelMode;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    class ItemMenu : RenderableComponent
    {
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
        private Sprite descriptionBox;
        private int scrollIndex;


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
            stats = SafeFileLoader.LoadStats();

            // ItemDisplay Config
            ItemMenuDisplay = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("ItemMenu")));
            ItemMenuDisplay.LocalOffset = new Vector2(Screen.Center.X - Entity.Transform.Position.X, Screen.Center.Y - Entity.Transform.Position.Y + 75); ;
            ItemMenuDisplay.Enabled = false;
            ItemMenuDisplay.LayerDepth = 0.1f;

            // selecButton config
            SelectionButton =
                Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            SelectionButton.LocalOffset =
                new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 - Entity.Transform.Position.X, Screen.Center.Y - 22 - Entity.Transform.Position.Y + 75);
            SelectionButton.Size = new Vector2(29, 55);
            SelectionButton.Enabled = false;

            // load texture
            descriptionBox = new Sprite(Entity.Scene.Content.LoadTexture("AttackOptions"));

            // adds ItemList to list of Spriterenderers
            Textures = new List<SpriteRenderer>();
            for (var i = 0; i < stats.itemList.Count; i++)
            {
                Textures.Add(Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture(stats.itemList[i].ItemType))));
                Textures[i].Size = new Vector2(27, 45);
                Textures[i].LocalOffset = GetLocationOfItem(i);
                    Textures[i].RenderLayer = 0;
                Textures[i].LayerDepth = 0;
                Textures[i].Enabled = false;
            }



        }

        private Vector2 GetLocationOfItem(int i)
        {
            return  new Vector2(Screen.Center.X - ItemMenuDisplay.Width / 2 + 40 * (i%6 + 1) - Entity.Transform.Position.X,
                    (float)((decimal)Screen.Center.Y - 30 + Math.Floor((decimal)i/6)*50 - (decimal)Entity.Transform.Position.Y + 75));

        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            SelectionButton.Enabled = true;
            ItemMenuDisplay.Enabled = true;
            for (var i= 0; i< 12; i++)
            {
                Textures[i+6*scrollIndex].Enabled = true;
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


            if (SelectionButton.LocalOffset.Y - 50 >= Screen.Center.Y - 30 - Entity.Transform.Position.Y + 75 && Up.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y - 50);
            }
            else if (scrollIndex > 0 && Up.IsPressed)
            {
                scrollIndex--;
                for (var i =0; i < Textures.Count; i++)
                {
                    Textures[i].SetLocalOffset(new Vector2(Textures[i].LocalOffset.X, Textures[i].LocalOffset.Y + 50));
                    if (i < 6) Textures[i + 6 * scrollIndex].Enabled = true;
                    if(i>=12 && i<18) Textures[i + 6 * scrollIndex].Enabled = false;
                }
            }
            if (SelectionButton.LocalOffset.Y + 50 <= Screen.Center.Y + 28 - Entity.Transform.Position.Y + 75 && Down.IsPressed)
            {
                SelectionButton.LocalOffset = new Vector2(SelectionButton.LocalOffset.X, SelectionButton.LocalOffset.Y + 50);
            }
            else if (stats.itemList.Count > 12 + 6 * scrollIndex && Down.IsPressed)
            {
                for (var i = 0; i < Textures.Count; i++)
                {
                    Textures[i].SetLocalOffset(new Vector2(Textures[i].LocalOffset.X, Textures[i].LocalOffset.Y - 50));
                    if (i < 6) Textures[i + 6 * scrollIndex].Enabled = false;
                    if (i >= 6 && i < 18) Textures[i + 6 * scrollIndex].Enabled = true;
                }
                scrollIndex++;
            }

            // choose selected item
            if (Enter.IsPressed)
            {
                var itemIndex = GetSelectedItemIndex();
                if (stats.energy - stats.itemList[itemIndex].cost >= 0)
                {
                    stats.itemList[itemIndex].ItemEffect(stats);
                    stats.itemList.RemoveAt(itemIndex);
                    Textures[itemIndex].Enabled = false;
                    Textures.RemoveAt(itemIndex);
                }
            }

            // Exit
            if (!ExitItemMenu.IsPressed) return;
            scrollIndex = 0;
            ItemMenuDisplay.Enabled = false;
            SelectionButton.Enabled = false;
            foreach (var element in Textures)
            {
                element.Enabled = false;
            }
            hud.Enabled = true;
            Enabled = false;

        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            if (GetSelectedItemIndex() != -1)
            {
                batcher.Draw(descriptionBox, new RectangleF((int)Screen.Center.X + 350 - 150 + 20, Screen.Center.Y + 40, 250, 150));
                batcher.DrawString(Graphics.Instance.BitmapFont, stats.itemList[GetSelectedItemIndex()].description, new Vector2((int)Screen.Center.X + 350 - 150 + 35, Screen.Center.Y + 30 + 60), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                for (var i = 0; i < Math.Abs(stats.itemList[GetSelectedItemIndex()].cost); i++)
                {
                    batcher.Draw(hud.EnergyStar, new Rectangle((int)Screen.Center.X + 350 - 150 + 35 + i * 25, (int)Screen.Center.Y + 30 + 35, 25, 25), stats.itemList[GetSelectedItemIndex()].cost > 0 ? Color.Red : Color.CornflowerBlue);
                }
            }
        }
        private int GetSelectedItemIndex()
        {
            for (var i = 0; i < Textures.Count; i++)
            {
                if (SelectionButton.LocalOffset.X == Textures[i].LocalOffset.X && SelectionButton.LocalOffset.Y - 8 == Textures[i].LocalOffset.Y)
                {
                    return i;
                }
            }
            return -1;
        }

        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;


    }
}
