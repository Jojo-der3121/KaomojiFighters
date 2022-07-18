using System;
using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Scenes.AlchemyScene
{
    class AlchemyHUD : RenderableComponent, IUpdatable
    {
        private VirtualButton ExitShop;
        private VirtualButton Purchase;
        private VirtualButton up;
        private VirtualButton down;
        private Texture2D AlchemicalPotion;
        private Texture2D Costar;
        private Entity AlchemyCircleEntity;
        private SpriteRenderer AlchemyCircle;
        private Stats stats;
        private int selectionIndexer;
        private int scrollIndex;
        private float rotatIndex;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ExitShop = new VirtualButton().AddKeyboardKey(Keys.Back);
            Purchase = new VirtualButton().AddKeyboardKey(Keys.Space);
            up = new VirtualButton().AddKeyboardKey(Keys.W);
            down = new VirtualButton().AddKeyboardKey(Keys.S);
            AlchemicalPotion = Entity.Scene.Content.LoadTexture("AlchemycalPotion");
            AlchemyCircleEntity = Entity.Scene.AddEntity(new Entity("AlchemyCircle"));
            AlchemyCircle = AlchemyCircleEntity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("AlchemyCircle")));
            AlchemyCircle.Size = new Vector2(100, 100);
            AlchemyCircle.LocalOffset = Screen.Center;
            AlchemyCircle.Enabled = false;
            Costar = Entity.Scene.Content.LoadTexture("CostStar");
            stats = SafeFileLoader.LoadStats();
            SetRenderLayer(-1);
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            if (AlchemyCircle != null)
            {
                AlchemyCircle.Enabled = false;
                AlchemyCircle.Size = new Vector2(100);
            }

        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            AlchemyCircle.Enabled = true;
            stats = SafeFileLoader.LoadStats();
        }


        public void Update()
        {
            if (up.IsPressed && selectionIndexer > 0) selectionIndexer--;
            else if (up.IsPressed && selectionIndexer == 0 && scrollIndex > 0) scrollIndex--;
            if (down.IsPressed && selectionIndexer < 9) selectionIndexer++;
            else if (down.IsPressed && selectionIndexer == 9 && stats.wordList.Count > 10 + scrollIndex) scrollIndex++;
            if (ExitShop.IsPressed) Enabled = false;
            if (Purchase.IsPressed && scrollIndex + selectionIndexer < stats.wordList.Count)
            {
                stats.itemList.Add(ParseWordToItem(stats.wordList[scrollIndex + selectionIndexer]));
                stats.wordList.RemoveAt(scrollIndex + selectionIndexer);
                SafeFileLoader.SaveStats(stats);
                scrollIndex = 0;
                selectionIndexer = 0;
            }

            AlchemyCircle.Transform.Rotation += 0.01f;
            AlchemyCircle.Size = Vector2.Lerp(AlchemyCircle.Size, new Vector2(1000), 0.06f);
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(new RectangleF(550, 200, 1320, 520), Color.Black);
            batcher.DrawRect(new RectangleF(560, 210, 1300, 500), new Color(104, 201, 52));
            batcher.DrawRect(new RectangleF(560, 210 + selectionIndexer * 500 / 10, 1300, 500 / 10), Color.DarkOliveGreen);
            batcher.DrawString(Graphics.Instance.BitmapFont, "<=", new Vector2(570, 220 + selectionIndexer * 500 / 10), Color.Black, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
            for (var i = 0; i < 10; i++)
            {
                if (i + scrollIndex < stats.wordList.Count) batcher.DrawString(Graphics.Instance.BitmapFont, stats.wordList[i + scrollIndex].actualWord, new Vector2(620, 220 + i * 500 / 10), Color.Black, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
            }

            batcher.DrawRect(new RectangleF(100, 200, 420, 520), Color.Black);
            batcher.DrawRect(new RectangleF(110, 210, 400, 500), new Color(104, 201, 52));
            batcher.Draw(AlchemicalPotion, new RectangleF(240, 250, 120, 180));
            batcher.DrawString(Graphics.Instance.BitmapFont, "AlchemycalPotion", new Vector2(150, 510), Color.Black, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
            for (var i = 0; i < Math.Abs(stats.wordList[selectionIndexer + scrollIndex].cost); i++)
            {
                batcher.Draw(Costar, new RectangleF(150 + i * 50, 450, 50, 50), stats.wordList[selectionIndexer + scrollIndex].cost > 0 ? Color.Red : Color.Blue);
            }
            batcher.DrawString(Graphics.Instance.BitmapFont, stats.wordList[selectionIndexer + scrollIndex].description, new Vector2(150, 540), Color.Black, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);

        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        public static Item ParseWordToItem(word word)
        {
            var item = new Item
            {
                cost = word.cost,
                description = word.description,
                itemEffect = word.wordEffect,
                _itemType = ItemType.AlchemycalPotion
            };
            return item;
        }
    }
}
