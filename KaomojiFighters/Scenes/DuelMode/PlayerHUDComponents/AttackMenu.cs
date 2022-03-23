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
        public Player player;
        private Texture2D TextButton;
        private Texture2D AttackOptionsMenu;
        private VirtualButton Up;
        private VirtualButton Down;
        private VirtualButton Enter;
        private VirtualButton ExitAttackMenu;
        private VirtualButton executeAttack;
        private int selectionY;
        private float selectedElement;
        private List<word> attackSentence;
        private Stats stat;
        
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            TextButton = Entity.Scene.Content.LoadTexture("TextButton");
            AttackOptionsMenu = Entity.Scene.Content.LoadTexture("AttackOptions");
            attackSentence = new List<word>();
            foreach (var item in player.WordList)
            {
                attackList.Add(Entity.AddComponent(new TextComponent()));
            }

            // define Buttons
            Up = new VirtualButton().AddKeyboardKey(Keys.W);
            Down = new VirtualButton().AddKeyboardKey(Keys.S);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);
            ExitAttackMenu = new VirtualButton().AddKeyboardKey(Keys.Back);
            executeAttack = new VirtualButton().AddKeyboardKey(Keys.Enter);

            selectionY = (int)Screen.Center.Y + TextButton.Height + 15;
            stat = player.GetComponent<Stats>();
        }
       

        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.Draw(TextButton, new Vector2(Screen.Center.X - TextButton.Width / 2, Screen.Center.Y));
            batcher.Draw(AttackOptionsMenu, new RectangleF(Screen.Center.X - 125, Screen.Center.Y + TextButton.Height + 10, 250, 150));
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 118, selectionY, 236, 25), Color.DarkOliveGreen);
            for (int i = 0; i < player.WordList.Count; i++)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, player.WordList[i].actualWord, new Vector2(Screen.Center.X - 115, Screen.Center.Y + TextButton.Height + 15 + i * 25), Color.Black, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < attackSentence.Count; i++)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, attackSentence[i].actualWord, new Vector2(Screen.Center.X - TextButton.Width / 2 + 10 + GetWordXLocation(i, attackSentence), Screen.Center.Y + 10), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
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
            if (Enter.IsPressed && IsAllowedWord(player.WordList[(int)selectedElement / 25].allowedPreviouseWords, attackSentence))
            {
                attackSentence.Add(player.WordList[(int)selectedElement / 25]);
            }
            else if (Enter.IsPressed && !IsAllowedWord(player.WordList[(int)selectedElement / 25].allowedPreviouseWords, attackSentence))
            {
                stat.HP -= 3;
            }

            if (executeAttack.IsPressed && attackSentence.Count >= 1)
            {
                if (attackSentence[attackSentence.Count - 1].isEnder)
                {
                    foreach (var element in attackSentence)
                    {
                        element.wordEffekt();
                    }
                   
                    TelegramService.SendPrivate(new Telegram(player.Entity.Name, "Kaomoji02", "auf die Fresse", "tach3tach3tach3")); //make later more generic
                    Core.Schedule(1.3f, (x) =>
                    {
                        TelegramService.SendPrivate(new Telegram(player.Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                        TelegramService.SendPrivate(new Telegram(player.Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));// maybe bug ? :thinking:
                        Enabled = false;
                    });
                    attackSentence.Clear();
                }
                else
                {
                    stat.HP -= 3;
                }
            }

            // Exit
            if (ExitAttackMenu.IsPressed)
            {
                Enabled = false;
            }
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
                if(sentence[i].typeOfWord == wordType.Nomen)
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
