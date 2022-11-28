using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace KaomojiFighters.Objects
{
    class EnemyListEntry
    {
        public string enemyName;
        public string weakness;
        public string immunity;
        public string entryTextureName;
        public Texture2D entryTexture;

        public EnemyListEntry(string entryName)
        {
            switch (entryName)
            {
                case "Kaomoji02":
                    enemyName = "Nano";
                    weakness = "MomJokes";
                    immunity = "insecure";
                    entryTextureName = "Kaomoji02";
                    break;
                case "Boss01":
                    enemyName = "Fortnite Kiddy"; 
                    weakness = "insecure";
                    immunity = "ptsd";
                    entryTextureName = "Kaomoji03";
                    break;
            }
        }
    }
}
