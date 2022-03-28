using KaomojiFighters.Enums;
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

        public List<TextComponent> attackList = new List<TextComponent>();
        private List<int> chosenAttackIndex = new List<int>();
        public HUD hud;
        public Player player;
        private Texture2D TextButton;
        private Texture2D AttackOptionsMenu;
        private VirtualButton Up;
        private VirtualButton Down;
        private VirtualButton Enter;
        private VirtualButton ExitAttackMenu;
        private VirtualButton executeAttack;
        private VirtualButton brickButton;
        private int selectionY;
        private float selectedElement;
        private List<word> attackSentence;
        private Stats stat;
        private Texture2D Bubble;
        private bool draw;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            Bubble = Entity.Scene.Content.LoadTexture("SpeachBubble");
            TextButton = Entity.Scene.Content.LoadTexture("TextButton");
            AttackOptionsMenu = Entity.Scene.Content.LoadTexture("AttackOptions");
            attackSentence = new List<word>();


            // define Buttons
            Up = new VirtualButton().AddKeyboardKey(Keys.W);
            Down = new VirtualButton().AddKeyboardKey(Keys.S);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            ExitAttackMenu = new VirtualButton().AddKeyboardKey(Keys.Back);
            executeAttack = new VirtualButton().AddKeyboardKey(Keys.Enter);
            brickButton = new VirtualButton().AddKeyboardKey(Keys.B);

            selectionY = (int)Screen.Center.Y + TextButton.Height + 15;
            stat = player.GetComponent<Stats>();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            foreach (var item in hud.Hand)
            {
                attackList.Add(Entity.AddComponent(new TextComponent()));
            }
        }


        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;



        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(TextButton, new Vector2(Screen.Center.X - TextButton.Width / 2, Screen.Center.Y));
            batcher.Draw(AttackOptionsMenu, new RectangleF(Screen.Center.X - 125, Screen.Center.Y + TextButton.Height + 10, 250, 150));
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 118, selectionY, 236, 25), Color.DarkOliveGreen);
            for (int i = 0; i < hud.Hand.Count; i++)
            {
                if (NotChosenAlready(i))
                {
                    batcher.DrawString(Graphics.Instance.BitmapFont, hud.Hand[i].actualWord, new Vector2(Screen.Center.X - 115, Screen.Center.Y + TextButton.Height + 15 + i * 25), Color.Black, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                }
            }
            for (int i = 0; i < attackSentence.Count; i++)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, attackSentence[i].actualWord, new Vector2(Screen.Center.X - TextButton.Width / 2 + 10 + GetWordXLocation(i, attackSentence), Screen.Center.Y + 10), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            }
            if (draw)
            {
                batcher.Draw(Bubble, new Rectangle((int)Screen.Center.X - 65 - GetWordXLocation(attackSentence.Count, attackSentence), 292, GetWordXLocation(attackSentence.Count, attackSentence), 50));
                for (int i = 0; i < attackSentence.Count; i++)
                {
                    batcher.DrawString(Graphics.Instance.BitmapFont, attackSentence[i].actualWord, new Vector2(Screen.Center.X - 50 - GetWordXLocation(attackSentence.Count, attackSentence) + GetWordXLocation(i, attackSentence), 300), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
                }
            }
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
            if (Enter.IsPressed && IsAllowedWord(hud.Hand[(int)selectedElement / 25].allowedPreviouseWords, attackSentence) && NotChosenAlready((int)selectedElement / 25) && stat.energy - hud.Hand[(int)selectedElement / 25].cost >= 0)
            {
                attackSentence.Add(hud.Hand[(int)selectedElement / 25]);
                chosenAttackIndex.Add((int)selectedElement / 25);
                stat.energy -= hud.Hand[(int)selectedElement / 25].cost;
            }
            else if (Enter.IsPressed && !IsAllowedWord(hud.Hand[(int)selectedElement / 25].allowedPreviouseWords, attackSentence))
            {
                stat.HP -= 3;
            }

            if (executeAttack.IsPressed && attackSentence.Count >= 1)
            {
                if (attackSentence[attackSentence.Count - 1].typeOfWord == wordType.Nomen )
                {
                    draw = true;
                    foreach (var element in attackSentence)
                    {
                        element.wordEffekt();
                    }

                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, "Kaomoji02", "auf die Fresse", "tach3tach3tach3")); //make later more generic
                    Core.Schedule(1.3f, (x) =>
                    {
                        draw = false;
                        attackSentence.Clear();
                        TelegramService.SendPrivate(new Telegram(player.Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                        TelegramService.SendPrivate(new Telegram(player.Entity.Name, player.Entity.Name, "its not your turn", "tach3tach3tach3"));// maybe bug ? :thinking:
                        Enabled = false;
                        hud.GY.AddRange(hud.Hand);
                        hud.Hand.Clear();
                        chosenAttackIndex.Clear();
                    });

                }
                else
                {
                    stat.HP -= 3;

                }
            }

            if (brickButton.IsPressed)
            {
                stat.HP -= 3;
                foreach (var element in attackSentence)
                {
                    stat.energy += element.cost;
                }
                Core.Schedule(1.3f, (x) =>
                {
                    draw = false;
                    attackSentence.Clear();
                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, player.Entity.Name, "its not your turn", "tach3tach3tach3"));// maybe bug ? :thinking:
                    Enabled = false;
                    hud.GY.AddRange(hud.Hand);
                    hud.Hand.Clear();
                    chosenAttackIndex.Clear();
                });
            }

            // Exit
            if (ExitAttackMenu.IsPressed)
            {
                foreach(var element in attackSentence)
                {
                    stat.energy += element.cost;
                }
                attackSentence.Clear();
                chosenAttackIndex.Clear();
                Enabled = false;
            }
        }

        private bool NotChosenAlready(int selection)
        {
            foreach (var e in chosenAttackIndex)
            {
                if (e == selection)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAllowedWord(List<wordType> allowedWords, List<word> sentenceWords)
        {
            foreach (var element in allowedWords)
            {
                if (sentenceWords.Count == 0 && wordType.nothing == element)
                {
                    return true;
                }
                else if (sentenceWords.Count > 0 && element == sentenceWords[sentenceWords.Count - 1].typeOfWord)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetWordXLocation(int e, List<word> sentence)
        {
            var length = 0;
            for (var i = 0; i < e; i++)
            {
                if (sentence[i].typeOfWord == wordType.Nomen)
                {
                    length += sentence[i].actualWord.Length * 15;
                }
                else
                {
                    length += sentence[i].actualWord.Length * 12;
                }

                length += (i + 1) * 15;
            }
            return length;
        }
    }
}
