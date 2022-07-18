using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace KaomojiFighters.Scenes.AlchemyScene
{
    class AGenerateHUD : RenderableComponent, IUpdatable
    {
        private int[] purchaseList = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private SpeechBubble[] speechBubbles = new SpeechBubble[10];
        private word[] wordArray = new[] { new word(Wort.You), new word(Wort.AFishHead), new word(Wort.And), new word(Wort.YourMom), new word(Wort.StepOn), new word(Wort.Legos), new word(Wort.I), new word(Wort.Hope), new word(Wort.Fucked), new word(Wort.DogFood) };
        private VirtualButton Exit;
        private VirtualButton up;
        private VirtualButton down;
        private VirtualButton left;
        private VirtualButton right;
        private VirtualButton select;
        private VirtualButton decreaseselection;
        private VirtualButton Purchase;
        private Vector2 selection = Vector2.Zero;
        private Vector2 selectionButtonPosition;
        private Stats stats;
        private Texture2D selectionButton;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            for (int i = 0; i < speechBubbles.Length; i++)
            {
                speechBubbles[i] = new SpeechBubble(new Vector2(Screen.Size.X / 5 - Screen.Center.X / 5 + i % 5 * Screen.Size.X / 5, i > 4 ? Screen.Size.Y / 3 * 2 : Screen.Size.Y / 3), wordArray[i].actualWord+" 0x", new Vector2(300, 200), true, 3);
            }

            up = new VirtualButton().AddKeyboardKey(Keys.W);
            down = new VirtualButton().AddKeyboardKey(Keys.S);
            left = new VirtualButton().AddKeyboardKey(Keys.A);
            right = new VirtualButton().AddKeyboardKey(Keys.D);
            select = new VirtualButton().AddKeyboardKey(Keys.Space);
            decreaseselection = new VirtualButton().AddKeyboardKey(Keys.X);
            Exit = new VirtualButton().AddKeyboardKey(Keys.Back);
            Purchase = new VirtualButton().AddKeyboardKey(Keys.Enter);
            stats = SafeFileLoader.LoadStats();
            selectionButton = Entity.Scene.Content.LoadTexture("AlchemyCircle");
            selectionButtonPosition = new Vector2(Screen.Size.X / 5 - Screen.Center.X / 5 - 160, Screen.Size.Y / 3 - 110);
            SetRenderLayer(-1);
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            stats = SafeFileLoader.LoadStats();
            selectionButtonPosition = new Vector2(Screen.Size.X / 5 - Screen.Center.X / 5  - 160, Screen.Size.Y / 3 - 130);
        }

        public void Update()
        {
            if (Exit.IsPressed) Enabled = false;
            if (up.IsPressed && selection.Y == 1) selection = new Vector2(selection.X, 0);
            if (down.IsPressed && selection.Y == 0) selection = new Vector2(selection.X, 1);
            if (left.IsPressed && selection.X > 0) selection = new Vector2(selection.X - 1, selection.Y);
            if (right.IsPressed && selection.X < 4) selection = new Vector2(selection.X + 1, selection.Y);
            if (@select.IsPressed) purchaseList[(int)(selection.X + selection.Y * 5)]++;
            if (decreaseselection.IsPressed && purchaseList[(int)(selection.X + selection.Y * 5)] > 0) purchaseList[(int)(selection.X + selection.Y * 5)]--;
            if (Purchase.IsPressed)
            {
                for (var i = 0; i < purchaseList.Length; i++)
                {
                    for (int e = 0; e < purchaseList[i]; e++)
                    {
                        stats.wordList.Add(wordArray[i]);
                    }
                }
                SafeFileLoader.SaveStats(stats);
                Enabled = false;
            }

        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            selectionButtonPosition = Vector2.Lerp(selectionButtonPosition, new Vector2(Screen.Size.X / 5 - Screen.Center.X / 5 + selection.X * Screen.Size.X / 5 - 160,
                selection.Y > 0 ? Screen.Size.Y / 3 * 2 - 130 : Screen.Size.Y / 3 - 130), 0.06f);
            batcher.Draw(selectionButton, new RectangleF(selectionButtonPosition, new Vector2(320, 270)));
            for (int i = 0; i < speechBubbles.Length; i++)
            {
                speechBubbles[i].GetSpeech(wordArray[i].actualWord+" "+purchaseList[i] + "x");
                speechBubbles[i].DrawTextField(batcher);
            }
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;
    }
}
