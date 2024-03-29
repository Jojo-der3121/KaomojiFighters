﻿using System.Collections.Generic;
using KaomojiFighters.Objects;
using Microsoft.Xna.Framework;
using Nez.Textures;

namespace KaomojiFighters.Mobs
{
    public class Stats
    {
        public string name;
        public int HP = 42;
        public int AttackValue = 1;
        public int Speed;
        public int energy = 7;
        public int Gold = 0;
        [Newtonsoft.Json.JsonIgnore]
       public (Sprite Normal,Sprite Attack, Sprite Hurt) sprites;   
        public int Defence = 1;
        public string weakness;
        public string immunity;
        public Vector2 OwOworldPosition;
        public List<word> wordList = new List<word>();
        public List<Item> itemList = new List<Item>();
        public List<string> knownEnemyList = new List<string>();

    }
    
}
