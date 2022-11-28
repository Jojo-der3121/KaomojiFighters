using System;
using System.Collections.Generic;
using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Scenes.OwOWorld
{
    class MenuSceneComponent : RenderableComponent, IUpdatable
    {
        private Stats stat;
        private Texture2D HP;
        private Texture2D ATK;
        private Texture2D DEF;
        private Texture2D SPD;
        private Texture2D ALCH;
        private Texture2D Costar;
        private SpeechBubble reiterOne;
        private SpeechBubble reiterZwei;
        private int scrollIndexI = 0;
        private int scrollIndexW = 0;
        private Vector2 selectionIndex = Vector2.Zero;
        private VirtualButton up;
        private VirtualButton down;
        private VirtualButton left;
        private VirtualButton right;
        private VirtualButton one;
        private VirtualButton two;
        private VirtualButton three;
        private VirtualButton close;
        private InventoryEnum inventoryState = InventoryEnum.items;
        private OwOWorldPlayer player;
        private SpeechBubble itemBubble;
        private SpeechBubble wordBubble;
        private SpeechBubble enemyBubble;
        private List<EnemyListEntry> knownEnemies;

        public MenuSceneComponent(OwOWorldPlayer pl)
        {
            player = pl;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            HP = Entity.Scene.Content.LoadTexture("HealthPotions");
            ATK = Entity.Scene.Content.LoadTexture("StrenghtPotions");
            DEF = Entity.Scene.Content.LoadTexture("DefencePotions");
            SPD = Entity.Scene.Content.LoadTexture("SpeedPotions");
            ALCH = Entity.Scene.Content.LoadTexture("AlchemycalPotion");
            Costar = Entity.Scene.Content.LoadTexture("CostStar");
            up = new VirtualButton().AddKeyboardKey(Keys.W);
            down = new VirtualButton().AddKeyboardKey(Keys.S);
            left = new VirtualButton().AddKeyboardKey(Keys.A);
            right = new VirtualButton().AddKeyboardKey(Keys.D);
            one = new VirtualButton().AddKeyboardKey(Keys.Q);
            two = new VirtualButton().AddKeyboardKey(Keys.E);
            three = new VirtualButton().AddKeyboardKey(Keys.R);
            close = new VirtualButton().AddKeyboardKey(Keys.Back);
            SetRenderLayer(4);
            itemBubble = new SpeechBubble(new Vector2(240 + 1420 / 6, 75), "Items (Q)", new Vector2(1420 / 3, 100), false, 4);
            wordBubble = new SpeechBubble(new Vector2(250 + 1420 / 3 + 1420 / 6, 75), "Words (E)", new Vector2(1420 / 3, 100), false, 4);
            enemyBubble = new SpeechBubble(new Vector2(260 + 1420 / 3 + 1420 / 3 + 1420 / 6, 75), "Enemies (R)", new Vector2(1420 / 3, 100), false, 4);
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            stat = SafeFileLoader.LoadStats();
            for (int i = 0; i < stat.knownEnemyList.Count; i++)
            {
                knownEnemies.Add(new EnemyListEntry(stat.knownEnemyList[i]));
                knownEnemies[i].entryTexture = Entity.Scene.Content.LoadTexture(knownEnemies[i].entryTextureName);
            }
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            player.Enabled = true;
            selectionIndex = Vector2.Zero;
            scrollIndexI = 0;
            scrollIndexW = 0;
            knownEnemies = new List<EnemyListEntry>();
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(0, 0, 1920, 1080, Color.Black * 0.35f);
            batcher.DrawRect(230, 130, 1460, 720, Color.Black);
            batcher.DrawRect(240, 140, 1440, 700, Color.CornflowerBlue);
            batcher.DrawRect(250, 150, 1420 / 3 - 10, 680, Color.Black);
            batcher.DrawRect(250 + 1420 / 3, 150, 1420 / 3 - 10, 680, Color.Black);
            if(inventoryState != InventoryEnum.enemies) batcher.DrawRect(250 + 1420 / 3 * 2, 150, 1420 / 3, 680, Color.Black);
            if (inventoryState != InventoryEnum.enemies) batcher.DrawRect(250 + (1420 / 6) * selectionIndex.X + (inventoryState == InventoryEnum.items ? 0 : 1420 / 3), 220 + 75 * selectionIndex.Y, 1420 / 6, 75, Color.CornflowerBlue);
            for (var i = 0; i < 16; i++)
            {
                if (stat.wordList.Count > i + scrollIndexW * 2) batcher.DrawString(Graphics.Instance.BitmapFont, stat.wordList[i + scrollIndexW * 2].actualWord, new Vector2(1420 / 3 + 300 + i % 2 * 200, 220 + (int)Math.Floor(i / 2f) * 75), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            for (var i = 0; i < 16; i++)
            {
                if (stat.itemList.Count > i + scrollIndexI * 2) batcher.Draw(GetItemTexture(stat.itemList[i + scrollIndexI * 2]._itemType), new Rectangle(350 + i % 2 * 150, 220 + (int)Math.Floor(i / 2f) * 75, 50, 75), Color.White);
            }
            itemBubble.DrawTextField(batcher);
            wordBubble.DrawTextField(batcher);
            enemyBubble.DrawTextField(batcher);

            if (inventoryState == InventoryEnum.items && stat.itemList.Count > selectionIndex.X + selectionIndex.Y * 2 + scrollIndexI * 2)
            {
                batcher.Draw(GetItemTexture(stat.itemList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexI * 2]._itemType), new Rectangle(240 + 1420 / 3 * 2 + 200 - 70, 230, 140, 220), Color.White);
                for (var i = 0; i < Math.Abs(stat.itemList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexI * 2].cost); i++)
                {
                    batcher.Draw(Costar, new RectangleF(1420 / 3 * 2 + 70 + i * 50 + 200, 470, 50, 50), stat.itemList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexI * 2].cost > 0 ? Color.Red : Color.Blue);
                }
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.itemList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexI * 2].GetType(), new Vector2(1420 / 3 * 2 + 150 + 200, 540), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.itemList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexI * 2].description, new Vector2(1420 / 3 * 2 + 150 + 200, 590), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            if (inventoryState == InventoryEnum.words && stat.wordList.Count > selectionIndex.X + selectionIndex.Y * 2 + scrollIndexW * 2)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].actualWord, new Vector2(1420 / 3 * 2 + 150 + 200 - 35, 350), Color.CornflowerBlue, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                for (var i = 0; i < Math.Abs(stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].cost); i++)
                {
                    batcher.Draw(Costar, new RectangleF(1420 / 3 * 2 + 70 + i * 50 + 200, 470, 50, 50), stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].cost > 0 ? Color.Red : Color.Blue);
                }
                batcher.DrawString(Graphics.Instance.BitmapFont, "Crit: " + stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].sensitivTopic, new Vector2(1420 / 3 * 2 + 150 + 200, 540), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].description, new Vector2(1420 / 3 * 2 + 150 + 200, 590), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.wordList[(int)(selectionIndex.X + selectionIndex.Y * 2) + scrollIndexW * 2].actualWord, new Vector2(270 + (1420 / 6) * selectionIndex.X + 1420 / 3, 235 + 75 * selectionIndex.Y), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);


            }
            if (inventoryState == InventoryEnum.enemies)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, knownEnemies[(int)selectionIndex.Y].enemyName, new Vector2(1420 / 3 * 2 + 150 + 250, 240), Color.Black, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
                batcher.Draw(knownEnemies[(int)selectionIndex.Y].entryTexture, new Rectangle(240 + 1420 / 3 * 2 + 200 - 100, 375, 310, 92), Color.White);
                batcher.DrawString(Graphics.Instance.BitmapFont,"Immunity: "+ knownEnemies[(int)selectionIndex.Y].immunity, new Vector2(1420 / 3 * 2 + 150 + 200, 540), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                batcher.DrawString(Graphics.Instance.BitmapFont,"Weakness: "+ knownEnemies[(int)selectionIndex.Y].weakness, new Vector2(1420 / 3 * 2 + 150 + 200, 590), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
        }

        private Texture2D GetItemTexture(ItemType i)
        {
            switch (i)
            {
                case ItemType.HealthPotion:
                    return HP;
                case ItemType.AttackPotion:
                    return ATK;
                case ItemType.DefencePotions:
                    return DEF;
                case ItemType.SpeedPotion:
                    return SPD;
                case ItemType.AlchemycalPotion:
                    return ALCH;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        public void Update()
        {
            if (one.IsPressed)
            {
                inventoryState = InventoryEnum.items;
                selectionIndex = Vector2.Zero;
                scrollIndexI = 0;
                scrollIndexW = 0;
            }
            if (two.IsPressed)
            {
                inventoryState = InventoryEnum.words;
                selectionIndex = Vector2.Zero;
                scrollIndexI = 0;
                scrollIndexW = 0;
            }
            if (three.IsPressed)
            {
                inventoryState = InventoryEnum.enemies;
                selectionIndex = Vector2.Zero;
                scrollIndexI = 0;
                scrollIndexW = 0;
            }

            if (inventoryState == InventoryEnum.enemies)
            {
                if(up.IsPressed && selectionIndex.Y > 0) selectionIndex = new Vector2(selectionIndex.X, selectionIndex.Y - 1);
                if(down.IsPressed && selectionIndex.Y < knownEnemies.Count-1) selectionIndex = new Vector2(selectionIndex.X, selectionIndex.Y + 1);
            }
            else
            {
                if (up.IsPressed && selectionIndex.Y > 0) selectionIndex = new Vector2(selectionIndex.X, selectionIndex.Y - 1);
                else if (up.IsPressed && inventoryState == InventoryEnum.items ? scrollIndexI > 0 : scrollIndexW > 0)
                {
                    if (inventoryState == InventoryEnum.items) scrollIndexI--;
                    else scrollIndexW--;
                }
                if (down.IsPressed && (inventoryState == InventoryEnum.items ? selectionIndex.Y + scrollIndexI < stat.itemList.Count / 2 : selectionIndex.Y + scrollIndexW < stat.wordList.Count / 2) && selectionIndex.Y < 7) selectionIndex = new Vector2(selectionIndex.X, selectionIndex.Y + 1);
                else if (down.IsPressed && (inventoryState == InventoryEnum.items
                    ? selectionIndex.Y + scrollIndexI < stat.itemList.Count / 2
                    : selectionIndex.Y + scrollIndexW < stat.wordList.Count / 2))
                {
                    if (inventoryState == InventoryEnum.items) scrollIndexI++;
                    else scrollIndexW++;
                }
                if (left.IsPressed && selectionIndex.X == 1) selectionIndex = new Vector2(0, selectionIndex.Y);
                if (right.IsPressed && selectionIndex.X == 0) selectionIndex = new Vector2(1, selectionIndex.Y);
            }
            if (close.IsPressed) Enabled = false;
        }
    }
}
