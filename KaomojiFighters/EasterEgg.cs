using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters
{
    class EasterEgg : Component, IUpdatable
    {
        public Keys[] EasterEggString;
        private List<Keys> OldInput = new List<Keys>();
        private List<Keys> NewInput = new List<Keys>();
        private List<Keys> KeyMemory = new List<Keys>();
    
        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.GetPressedKeyCount() != 1) return;
            NewInput.AddRange(keyboardState.GetPressedKeys());
            foreach( var element in NewInput)
            {
                if (!OldInput.Contains(element))
                {
                    KeyMemory.Add(element);
                }
            }

            OldInput.Clear();
            OldInput.AddRange(NewInput);
            NewInput.Clear();

            if(KeyMemory.Count > 25)
            {
                for (int i = 0; i < 7; i++)
                {
                    KeyMemory.RemoveAt(0);
                }
            }

            if (KeyMemory.Count < EasterEggString.Length) return;
            for (int i = 0; i < EasterEggString.Length-1; i++)
            {
                if (KeyMemory[KeyMemory.Count - i - 1] != EasterEggString[EasterEggString.Length - i - 1]) return;
            }
            TelegramService.SendPrivate(new Telegram(Entity.Name,Entity.Name,"Frohe Ostern","tach3tach3tach3"));
        }

        // Booti please plae DMC2 dante is gud he shuut da gunn
    }
}
