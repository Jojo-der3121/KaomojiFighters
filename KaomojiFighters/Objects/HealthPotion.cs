using KaomojiFighters.Mobs;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Objects
{
    abstract class Item : Component
    {
        public string ItemType { get; set; }
        public int cost;
        public Stats stats;
        
        public virtual void ItemEffect()
        {
            var stats = Entity.GetComponent<Stats>();
            stats.energy -= cost;
        }
    }
    class HealthPotion : Item
    {
       public HealthPotion()
        {
            ItemType = "HealthPotions";
            cost = 7;
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.HP += 15;
        }
    }

    class SpeedPotion : Item
    {
      public SpeedPotion()
        {
            ItemType = "SpeedPotions";
            cost = -10;
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.Speed += 20;
        }
    }

    class StrenghtPotion : Item
    {
        public StrenghtPotion()
        {
            ItemType = "StrenghtPotions";
            cost = 5;
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.AttackValue += 5;
        }
    }
}
