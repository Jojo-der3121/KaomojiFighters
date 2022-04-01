using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Enums;
using KaomojiFighters.Objects;
using KaomojiFighters.Scenes.DuelMode;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    public class Stats
    {

        public int HP = 42;
        public int AttackValue = 1;
        public int Speed;
        public int energy = 7;
        [Newtonsoft.Json.JsonIgnore]
       public (Sprite Normal,Sprite Attack, Sprite Hurt) sprites;   
        public int Defence = 1;
        public string weakness;
        public Vector2 OwOworldPosition;
        public List<word> wordList = new List<word>();
        public List<Item> itemList = new List<Item>();

    }
    
}
