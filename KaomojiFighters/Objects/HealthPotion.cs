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
        [JsonIgnore]
        public int cost;
        [JsonIgnore]
        public string description;
        [JsonIgnore]
        public Action<Stats> itemEffect;

        private Wort BaseWort;

        [JsonConstructor]
        public Item()
        {

        }

        public Item(ItemType itemType, Wort wort)
        {
            switch (itemType)
            {
                case ItemType.HealthPotion:
                    description = "recovers 15 HP";
                    cost = 7;
                    itemEffect = (x) => x.HP += 15;
                    _itemType = itemType;
                    break;
                case ItemType.AttackPotion:
                    _itemType = itemType;
                    description = "increases ATK by 5";
                    cost = 5;
                    itemEffect = (x) => x.AttackValue += 5;
                    break;
                case ItemType.DefencePotions:
                    _itemType = itemType;
                    description = "increases Def by 3";
                    cost = 6;
                    itemEffect = (x) => x.Defence += 3;
                    break;
                case ItemType.SpeedPotion:
                    _itemType = itemType;
                    description = "recovers 9 Energy";
                    cost = -9;
                    break;
                case ItemType.AlchemycalPotion:
                    var cacheWord = new word(wort);
                    var item = AlchemyHUD.ParseWordToItem(cacheWord);
                    _itemType = itemType;
                    description = item.description;
                    cost = item.cost;
                    itemEffect = item.itemEffect;

                    break;
            }
            BaseWort = wort;
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

        public static List<Item> GetItemList(List<Item> cacheItemList)
        {
            var itemList = new List<Item>();
            foreach (var item in cacheItemList)
            {
                itemList.Add(new Item(item._itemType, item.BaseWort));
            }
            return itemList;
        }
    }

}
