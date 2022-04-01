using KaomojiFighters.Mobs;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Objects
{
    public abstract class Item 
    {
        public string ItemType { get; set; }
        public int cost;
        public string description;
        
        public virtual void ItemEffect(Stats stats)
        {
            stats.energy -= cost;
        }
    }
    class HealthPotion : Item
    {
       public HealthPotion()
        {
            ItemType = "HealthPotions";
            description = "recovers 15 HP";
            cost = 7;
        }

        public override void ItemEffect(Stats stats)
        {
            base.ItemEffect(stats);
            stats.HP += 15;
        }
    }

    class SpeedPotion : Item
    {
      public SpeedPotion()
        {
            ItemType = "SpeedPotions";
            description = "recovers 9 Energy";
            cost = -9;
        }
    }

    class StrenghtPotion : Item
    {
        public StrenghtPotion()
        {
            ItemType = "StrenghtPotions";
            description = "increases ATK by 5";
            cost = 5;
        }

        public override void ItemEffect(Stats stats)
        {
            base.ItemEffect(stats);
            stats.AttackValue += 5;
        }
    }
}
