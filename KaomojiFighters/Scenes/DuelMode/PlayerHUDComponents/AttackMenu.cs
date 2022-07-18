using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents
{
    class AttackMenu : RenderableComponent
    {
        private List<TextComponent> attackList = new List<TextComponent>();
        private readonly List<int> chosenAttackIndex = new List<int>();
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
        private bool draw;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
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
            stat = player.stat;
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
        {   // draws attack selection
            var str = GetString(attackSentence);
            batcher.DrawRect( new RectangleF(Screen.Center.X - TextButton.Width / 2, Screen.Center.Y, Math.Max(300 , Graphics.Instance.BitmapFont.MeasureString(str).X * 3)+20, TextButton.Height), new Color(104,201,52));
            batcher.Draw(AttackOptionsMenu, new RectangleF(Screen.Center.X - 125, Screen.Center.Y + TextButton.Height + 10, 250, 150));
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 118, selectionY, 236, 25), Color.DarkOliveGreen);

            // draws selection Descriptions
            if (NotChosenAlready((int)selectedElement / 25) && (int)selectedElement / 25 < hud.Hand.Count)
            {
                batcher.Draw(AttackOptionsMenu, new RectangleF(Screen.Center.X + 135, Screen.Center.Y + TextButton.Height + 10, 250, 150));
                batcher.DrawString(Graphics.Instance.BitmapFont, hud.Hand[(int)selectedElement / 25].description, new Vector2(Screen.Center.X + 150, Screen.Center.Y + TextButton.Height + 60), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

                for (var i = 0; i < Math.Abs(hud.Hand[(int)selectedElement / 25].cost); i++)
                {
                    batcher.Draw(hud.EnergyStar, new Rectangle((int)Screen.Center.X + 150 + i * 25, (int)Screen.Center.Y + TextButton.Height + 35, 25, 25), hud.Hand[(int)selectedElement / 25].cost > 0 ? Color.Red : Color.CornflowerBlue);
                }
            }


            // draws all non selected words
            for (int i = 0; i < hud.Hand.Count; i++)
            {
                if (NotChosenAlready(i))
                {
                    batcher.DrawString(Graphics.Instance.BitmapFont, hud.Hand[i].actualWord, new Vector2(Screen.Center.X - 115, Screen.Center.Y + TextButton.Height + 15 + i * 25), Color.Black, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                }
            }
            // draws the attacksentece
            batcher.DrawString(Graphics.Instance.BitmapFont, str, new Vector2(Screen.Center.X - TextButton.Width / 2 + 10, Screen.Center.Y + 10), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            //draws the attack speechBubble
            if (draw)
            {
                var bubble = new SpeechBubble(new Vector2(700, 300),str, new Vector2((int)Graphics.Instance.BitmapFont.MeasureString(str).X * 3 + 60, (int)Graphics.Instance.BitmapFont.MeasureString(str).Y + 100), false, 3);
                bubble.DrawTextField(batcher);
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

            if (hud.Hand.Count-1 >= selectedElement / 25)
            {
                if (Enter.IsPressed &&
                    (IsAllowedWord(hud.Hand[(int) selectedElement / 25].allowedPreviouseWords, attackSentence) &&
                     NotChosenAlready((int) selectedElement / 25) &&
                     stat.energy - hud.Hand[(int) selectedElement / 25].cost >= 0))
                {
                    attackSentence.Add(hud.Hand[(int) selectedElement / 25]);
                    chosenAttackIndex.Add((int) selectedElement / 25);
                    stat.energy -= hud.Hand[(int) selectedElement / 25].cost;
                }
                else if (Enter.IsPressed && !IsAllowedWord(hud.Hand[(int) selectedElement / 25].allowedPreviouseWords,
                    attackSentence))
                {
                    stat.HP -= 3;
                }
            }
            else if (Enter.IsPressed)
            {
                stat.HP -= 3;
            }

            if (executeAttack.IsPressed && attackSentence.Count >= 1)
            {
                if (attackSentence[attackSentence.Count - 1].typeOfWord == wordType.Nomen)
                {
                    draw = true;
                    var cacheString = "";
                    foreach (var element in attackSentence)
                    {
                        cacheString += element.sensitivTopic + " ";
                        element.ExecuteEffect(stat);
                    }

                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, "Kaomoji02", "auf die Fresse", cacheString)); //make later more generic
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
                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, player.Entity.Name, "its not your turn", "tach3tach3tach3"));
                    Enabled = false;
                    hud.GY.AddRange(hud.Hand);
                    hud.Hand.Clear();
                    chosenAttackIndex.Clear();
                });
            }

            // Exit
            if (!ExitAttackMenu.IsPressed) return;
            {
                foreach (var element in attackSentence)
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

        private bool IsAllowedWord(List<wordType> allowedWords, List<word> sentenceWords)
        {
            foreach (var element in allowedWords)
            {
                if (sentenceWords.Count == 0 && wordType.nothing == element)
                {
                    return true;
                } 
                if (sentenceWords.Count > 0 && element == sentenceWords[sentenceWords.Count - 1].typeOfWord)
                {
                    return true;
                }
            }
            return false;
        }

       

        public string GetString( List<word> sentence) =>  String.Join(" ",sentence.Select((x) => x.actualWord).ToArray());
        
    }
}
