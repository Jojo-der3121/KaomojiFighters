using KaomojiFighters.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using KaomojiFighters.Mobs;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework.Graphics;
using Nez.BitmapFonts;

namespace KaomojiFighters.Scenes.SellerScene
{
    class ShopHUD : RenderableComponent, IUpdatable
    {
        private VirtualButton ExitShop;
        private VirtualButton Purchase;
        private VirtualButton up;
        private VirtualButton down;
        private VirtualButton select;
        private VirtualButton decreaseSelect;
        private Texture2D HealthPotion;
        private Texture2D AttackPotion;
        private Texture2D DefencePotion;
        private Texture2D SpeedPotion;
        private int[] buyPriceArray = new int[] { 10, 5, 2, 13 };
        private int[] sellPriceArray = new int[] { 8, 3, 1, 9 };
        private int[] selectedProduct = new int[] { 0, 0, 0, 0 };
        private int[] availableItems = new int[] { 0, 0, 0, 0 };
        private Stats stats;
        public bool buy;
        private Shop shop;
        private SpeechBubble bubble;
        private string[] buyAnswers = new[] { "Porsche Cayman S!! wollen sie ihn auch ? Geben sie mir ihr ganzes Geld und schon gestern wird er deiner sein.", "Um Geld zu gewinnen muss man Geld ausgeben, die weisen Wort des Gottes des Geldes. Eines Tages wird er sich meiner erbarmen.","Jede spende dafuer mich zum Millionaer zu machen ist eine Investiton in ihre Zukunft. Haendler for President!!", "KOMM IN DIE GRUPPE! ich weiss zwar nicht warum ich das sagen soll jedoch hatte es mir mein Finanzberater geraten, Porsche du wirst mein sein !!", "Duerfte ich sie in meine neue Crypto Waehrung interessieren ?" };
        private string[] sellAnswers = new[] {"Ich bitte dich ich habe Frau und Kinder ..", "sobald ich President bin werde ich mich fuer diese Sabotage Rechen! Mark my Words Vengeance will be Mine!", " NICHT MEINE DOGDECOIN!? i'll allways remember their memories. Von dem Moment an wo ich sie ge- ,\"verdient\" habe, bis zu den 10 Jahren wo ich sie aufbewahrt habe und ihr Wert zu 1/10 des vorherigen wurde. You don't happen to have any alcohol on you, do you?", "Darf ich sie in Steam Gutscheinkarten bezhlen?"};
        private int selectionIndex;

        public ShopHUD(Shop originShop)
        {
            shop = originShop;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ExitShop = new VirtualButton().AddKeyboardKey(Keys.Back);
            Purchase = new VirtualButton().AddKeyboardKey(Keys.Enter);
            up = new VirtualButton().AddKeyboardKey(Keys.W);
            down = new VirtualButton().AddKeyboardKey(Keys.S);
            @select = new VirtualButton().AddKeyboardKey(Keys.Space);
            decreaseSelect = new VirtualButton().AddKeyboardKey(Keys.X);

            HealthPotion = Entity.Scene.Content.LoadTexture("HealthPotions");
            AttackPotion = Entity.Scene.Content.LoadTexture("StrenghtPotions");
            DefencePotion = Entity.Scene.Content.LoadTexture("DefencePotions");
            SpeedPotion = Entity.Scene.Content.LoadTexture("SpeedPotions");
            stats = SafeFileLoader.LoadStats();
            shop.Gold = stats.Gold;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            var r = new System.Random();
            bubble = new SpeechBubble(new Vector2(buy ? Screen.Size.X / 7 : Screen.Size.X / 7 * 6, Screen.Center.Y), buy ?  buyAnswers[r.Next(buyAnswers.Length-1)] : sellAnswers[r.Next(sellAnswers.Length - 1)], new Vector2(400, 960), false, 4);
            stats = SafeFileLoader.LoadStats();
            if (buy) return;
            for (var i = 0; i < selectedProduct.Length; i++)
            {
                var searchedItemType = GetItemTyp(i);
                foreach (var item in stats.itemList)
                {
                    if (item._itemType == searchedItemType._itemType) availableItems[i]++;
                }
            }
        }

        private Item GetItemTyp(int i)
        {
            switch (i)
            {
                case 0:
                    return new Item(ItemType.HealthPotion, Wort.StepOn);
                case 1:
                    return new Item(ItemType.AttackPotion, Wort.StepOn);
                case 2:
                    return new Item(ItemType.DefencePotions, Wort.StepOn);
                case 3:
                    return new Item(ItemType.SpeedPotion, Wort.StepOn);
            }
            return null;
        }

        private int GetFullPrice()
        {
            return buyPriceArray[0] * selectedProduct[0] + buyPriceArray[1] * selectedProduct[1] +
                   buyPriceArray[2] * selectedProduct[2] + buyPriceArray[3] * selectedProduct[3]+ buyPriceArray[selectionIndex];
        }

        public void Update()
        {
            if (@select.IsPressed && buy && GetFullPrice() <= stats.Gold || @select.IsPressed && !buy && availableItems[selectionIndex] >= selectedProduct[selectionIndex] + 1)selectedProduct[selectionIndex]++;
            if (decreaseSelect.IsPressed && selectedProduct[selectionIndex]>0)selectedProduct[selectionIndex]--;
            if (up.IsPressed && selectionIndex > 0) selectionIndex--;
            if (down.IsPressed && selectionIndex < 3) selectionIndex++;
            if (Purchase.IsPressed)
            {
                for (var i = 0; i < selectedProduct.Length; i++)
                {
                    var itemTyp = GetItemTyp(i);
                    for (var e = selectedProduct[i]; e > 0; e--)
                    {
                        if (buy)stats.itemList.Add(itemTyp);
                        else
                        {
                            for (var v = 0; v < stats.itemList.Count; v++)
                            {
                                if (stats.itemList[v]._itemType != itemTyp._itemType) continue;
                                stats.itemList.RemoveAt(v);
                                break;
                            }
                        }
                    }
                }
                if(buy) stats.Gold -= (GetFullPrice()- buyPriceArray[selectionIndex]);
                else stats.Gold += sellPriceArray[0] * selectedProduct[0] + sellPriceArray[1] * selectedProduct[1] +
                              sellPriceArray[2] * selectedProduct[2] + sellPriceArray[3] * selectedProduct[3];
                SafeFileLoader.SaveStats(stats);
                shop.Gold = stats.Gold;
                selectedProduct = new int[] { 0, 0, 0, 0 };
                availableItems = new int[] { 0, 0, 0, 0 };
                selectionIndex = 0;
                Enabled = false;
            }
            if (ExitShop.IsPressed)
            {
                selectedProduct = new int[] { 0, 0, 0, 0 };
                availableItems = new int[] { 0, 0, 0, 0 };
                selectionIndex = 0;
                Enabled = false;
            }
        }

        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(buy ? 1620 - 140 : 140, 30, 300f, 200f, Color.Black);
            batcher.DrawRect(buy ? 1620 - 130 : 150, 40, 280f, 180f, new Color(104, 201, 52));
            batcher.DrawRect(buy ? 1620 - 140 : 140, 30 + 220, 300f, 200f, Color.Black);
            batcher.DrawRect(buy ? 1620 - 130 : 150, 40 + 220, 280f, 180f, new Color(104, 201, 52));
            batcher.DrawRect(buy ? 1620 - 140 : 140, 30 + 220 * 2, 300f, 200f, Color.Black);
            batcher.DrawRect(buy ? 1620 - 130 : 150, 40 + 220 * 2, 280f, 180f, new Color(104, 201, 52));
            batcher.DrawRect(buy ? 1620 - 140 : 140, 30 + 220 * 3, 300f, 200f, Color.Black);
            batcher.DrawRect(buy ? 1620 - 130 : 150, 40 + 220 * 3, 280f, 180f, new Color(104, 201, 52));
            batcher.DrawRect(buy ? 1620 - 130 : 150, 40 + 220 * selectionIndex, 280f, 180f, Color.DarkGreen);
            
            batcher.Draw(HealthPotion, new RectangleF(buy ? 1620 - 50 : 230, 50 + 30, 75, 100));
            if (selectedProduct[0] > 0) batcher.DrawString(Graphics.Instance.BitmapFont, selectedProduct[0] + "x", new Vector2(buy ? 1620 - 100 : 180, 50 + 75), Color.Black, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (!buy) batcher.DrawString(Graphics.Instance.BitmapFont, availableItems[0] + "x", new Vector2(180 + 125, 50 + 75 + 30), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, buy ? buyPriceArray[0].ToString() : sellPriceArray[0].ToString(), new Vector2(buy ? 1620 + 60 : 340, 50 + 75), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            batcher.Draw(AttackPotion, new RectangleF(buy ? 1620 - 50 : 230, 50 + 30 + 220, 75, 100));
            if (selectedProduct[1] > 0) batcher.DrawString(Graphics.Instance.BitmapFont, selectedProduct[1] + "x", new Vector2(buy ? 1620 - 100 : 180, 50 + 75 + 220), Color.Black, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (!buy) batcher.DrawString(Graphics.Instance.BitmapFont, availableItems[1] + "x", new Vector2(180 + 125, 50 + 75 + 30 + 220), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, buy ? buyPriceArray[1].ToString() : sellPriceArray[1].ToString(), new Vector2(buy ? 1620 + 60 : 340, 50 + 75 +220), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            batcher.Draw(DefencePotion, new RectangleF(buy ? 1620 - 50 : 230, 50 + 30 + 220 * 2, 75, 100));
            if (selectedProduct[2] > 0) batcher.DrawString(Graphics.Instance.BitmapFont, selectedProduct[2] + "x", new Vector2(buy ? 1620 - 100 : 180, 50 + 75 + 220 * 2), Color.Black, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (!buy) batcher.DrawString(Graphics.Instance.BitmapFont, availableItems[2] + "x", new Vector2(180 + 125, 50 + 75 + 30 + 220 * 2), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, buy ? buyPriceArray[2].ToString() : sellPriceArray[2].ToString(), new Vector2(buy ? 1620 + 60 : 340, 50 + 75 + 220*2), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);

            batcher.Draw(SpeedPotion, new RectangleF(buy ? 1620 - 50 : 230, 50 + 30 + 220 * 3, 75, 100));
            if (selectedProduct[3] > 0) batcher.DrawString(Graphics.Instance.BitmapFont, selectedProduct[3] + "x", new Vector2(buy ? 1620 - 100 : 180, 50 + 75 + 220 * 3), Color.Black, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            if (!buy) batcher.DrawString(Graphics.Instance.BitmapFont, availableItems[3] + "x", new Vector2(180 + 125, 50 + 75 + 30 + 220 * 3), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, buy ? buyPriceArray[3].ToString() : sellPriceArray[3].ToString(), new Vector2(buy ? 1620 + 60 : 340, 50 + 75 + 220*3), Color.PaleGoldenrod, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
            bubble.DrawTextField(batcher);
        }
    }
}
