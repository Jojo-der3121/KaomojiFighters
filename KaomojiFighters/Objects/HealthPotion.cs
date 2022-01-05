using KaomojiFighters.Mobs;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Objects
{
    public class Item : Component
    {
        public string ItemType { get; set; }
        public virtual void ItemEffect()
        {

        }
    }
    public class HealthPotion : Item
    {
        private Stats stats;

        public HealthPotion()
        {
            ItemType = "HealthPotions";
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.HP += 15;
        }
    }

    public class SpeedPotion : Item
    {
        private Stats stats;

        public SpeedPotion()
        {
            ItemType = "SpeedPotions";
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.Speed += 20;
        }
    }

    public class StrenghtPotion : Item
    {
        private Stats stats;

        public StrenghtPotion()
        {
            ItemType = "StrenghtPotions";
        }

        public override void ItemEffect()
        {
            base.ItemEffect();
            stats = Entity.GetComponent<Stats>();
            stats.AttackValue += 5;
        }
    }
}
