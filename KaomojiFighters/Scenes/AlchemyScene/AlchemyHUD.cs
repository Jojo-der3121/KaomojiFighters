using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Scenes.AlchemyScene
{
    class AlchemyHUD : RenderableComponent, IUpdatable
    {
        private VirtualButton ExitShop;
        private VirtualButton Purchase;
        private VirtualButton up;
        private VirtualButton down;
        private Texture2D AlchemicalPotion;
        private Stats stats;
        private SpeechBubble bubble;
        private int selectionIndexer;
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ExitShop = new VirtualButton().AddKeyboardKey(Keys.Back);
            Purchase = new VirtualButton().AddKeyboardKey(Keys.Enter);
            up = new VirtualButton().AddKeyboardKey(Keys.W);
            down = new VirtualButton().AddKeyboardKey(Keys.S);
            AlchemicalPotion = Entity.Scene.Content.LoadTexture("AlchemycalPotion");
            stats = SafeFileLoader.LoadStats();
        }

        public void Update()
        {
            if (up.IsPressed && selectionIndexer > 0)selectionIndexer--;
            if (down.IsPressed && selectionIndexer < stats.wordList.Count - 1) selectionIndexer++;
            if (ExitShop.IsPressed) Enabled = false;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(new RectangleF(550,200, 1320, 520), Color.Black);
            batcher.DrawRect(new RectangleF(560,210,1300,500), new Color(104, 201, 52));
            batcher.DrawRect(new RectangleF(560, 210+selectionIndexer* 500 / 10, 1300, 500/10), Color.DarkOliveGreen);

            batcher.DrawRect(new RectangleF(100, 200, 420, 520), Color.Black);
            batcher.DrawRect(new RectangleF(110, 210, 400, 500), new Color(104, 201, 52));
            batcher.Draw(AlchemicalPotion, new RectangleF(240,250,120,180));

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
