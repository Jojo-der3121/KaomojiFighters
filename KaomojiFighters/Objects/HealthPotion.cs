using System;
using System.Collections.Generic;
using KaomojiFighters.Enums;
using KaomojiFighters.Mobs;
using KaomojiFighters.Scenes.AlchemyScene;
using Newtonsoft.Json;

namespace KaomojiFighters.Objects
{
    public class Item
    {
        public ItemType _itemType { get; set; }
        public Wort BaseWort { get; set; }
        [JsonIgnore]
        public int cost;
        [JsonIgnore]
        public string description;
        [JsonIgnore]
        public Action<Stats> itemEffect;

        

        [JsonConstructor]
        public Item()
        {

        }

        public Item(ItemType itemType, Wort wort)
        {
            _itemType = itemType;
            BaseWort = wort;
            GetRightItemProperties();
        }

        public void GetRightItemProperties()
        {
            switch (_itemType)
            {
                case ItemType.HealthPotion:
                    description = "recovers 15 HP";
                    cost = 7;
                    itemEffect = (x) => x.HP += 15;
                    break;
                case ItemType.AttackPotion:
                    description = "increases ATK by 5";
                    cost = 5;
                    itemEffect = (x) => x.AttackValue += 5;
                    break;
                case ItemType.DefencePotions:
                    description = "increases Def by 3";
                    cost = 6;
                    itemEffect = (x) => x.Defence += 3;
                    break;
                case ItemType.SpeedPotion:
                    description = "recovers 9 Energy";
                    cost = -9;
                    break;
                case ItemType.AlchemycalPotion:
                    var cacheWord = new word(BaseWort);
                    var item = AlchemyHUD.ParseWordToItem(cacheWord);
                    description = item.description;
                    cost = item.cost;
                    itemEffect = item.itemEffect;
                    break;
            }
        }

        public string GetType()
        {
            switch (_itemType)
            {
                case ItemType.HealthPotion:
                    return "HealthPotions";
                case ItemType.AttackPotion:
                    return "StrenghtPotions";
                case ItemType.DefencePotions:
                    return "DefencePotions";
                case ItemType.SpeedPotion:
                    return "SpeedPotions";
                case ItemType.AlchemycalPotion:
                    return "AlchemycalPotion";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}
