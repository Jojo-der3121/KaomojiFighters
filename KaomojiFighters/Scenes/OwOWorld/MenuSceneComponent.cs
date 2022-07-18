using System;
using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
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
        private int scrollIndex = 0;
        private Vector2 selectionIndex = Vector2.Zero;
        private VirtualButton up;
        private VirtualButton down;
        private VirtualButton left;
        private VirtualButton right;
        private VirtualButton one;
        private VirtualButton two;
        private VirtualButton close;
        private bool IsItem = true;
        private OwOWorldPlayer player;
        private SpeechBubble itemBubble;
        private SpeechBubble wordBubble;

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
            one = new VirtualButton().AddKeyboardKey(Keys.Q);
            two = new VirtualButton().AddKeyboardKey(Keys.E);
            close = new VirtualButton().AddKeyboardKey(Keys.Back);
            SetRenderLayer(4);
            itemBubble = new SpeechBubble(new Vector2(240 + 1420 / 6, 75), "Items (Q)",new Vector2( 1420 / 3, 100),false,4);
            wordBubble = new SpeechBubble(new Vector2(250+1420/3+ 1420 / 6, 75), "Words (E)", new Vector2(1420 / 3, 100), false,4);
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            stat = SafeFileLoader.LoadStats();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            player.Enabled = true;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(0,0,1920,1080, Color.Black*0.35f);
            batcher.DrawRect(230,130,1460,720, Color.Black);
            batcher.DrawRect(240,140,1440,700, Color.CornflowerBlue);
            batcher.DrawRect(250,150,1420 / 3 -10,680, Color.Black);
            batcher.DrawRect(250+ 1420 / 3 , 150,1420 / 3 -10,680, Color.Black);
            batcher.DrawRect(250+ 1420 / 3*2, 150,1420 / 3,680, Color.Black);
            for (var i = 0; i < stat.wordList.Count; i++)
            {
                batcher.DrawString( Graphics.Instance.BitmapFont, stat.wordList[i].actualWord, new Vector2(1420 / 3+300 + i % 2 * 200, 220 + (int)Math.Floor(i / 2f) * 75), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < stat.wordList.Count; i++)
            {
                batcher.Draw(GetItemTexture(stat.itemList[i]._itemType), new Rectangle(350 + i%2 * 150, 220+ (int)Math.Floor(i/2f)*75, 50, 75), Color.White);
            }
            itemBubble.DrawTextField(batcher);
            wordBubble.DrawTextField(batcher);

            if (IsItem)
            {
                batcher.Draw(GetItemTexture(stat.itemList[0]._itemType), new Rectangle(240+ 1420 / 3 * 2 + 200-70, 230, 140, 220), Color.White);
                for (var i = 0; i < Math.Abs(stat.wordList[0].cost); i++)
                {
                    batcher.Draw(Costar, new RectangleF(1420 / 3 * 2 + 150 + i * 50 + 200, 470, 50, 50), stat.wordList[0].cost > 0 ? Color.Red : Color.Blue);
                }
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.itemList[0].GetType(), new Vector2(1420 / 3 * 2 + 150 + 200, 540), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                batcher.DrawString(Graphics.Instance.BitmapFont, stat.itemList[0].description, new Vector2(1420 / 3 * 2 + 150 + 200, 590), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            else
            {

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
            if (one.IsPressed) IsItem = true;
            if (two.IsPressed) IsItem = false;
            if (close.IsPressed) Enabled = false;
            
            
        }
    }
}
