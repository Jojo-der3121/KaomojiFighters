using KaomojiFighters.Mobs;
using Newtonsoft.Json;
using System.IO;

namespace KaomojiFighters
{
    class SafeFileLoader
    {
        public static Stats LoadStats()
        {
            if(! File.Exists("PlayerStats.json")) return new Stats() { Speed = 8, wordList = word.GetWordList()};
            var content = File.ReadAllText("PlayerStats.json");
            var deserializedStats =JsonConvert.DeserializeObject<Stats>(content);
            foreach(var word in deserializedStats.wordList)
            {
                word.GetRightWordProperties();
            } 
            foreach (var item in deserializedStats.itemList)
            {
                item.GetRightItemProperties();
            }
            return deserializedStats;
        }

        public static void SaveStats(Stats stats)
        {
            var jsonString = JsonConvert.SerializeObject(stats);
            File.WriteAllText("PlayerStats.json", jsonString);
        }

        public static int[] GetOwOWorldSafeFile()
        {
            if (!File.Exists("OwOWorldSafeFile.json")) return new int[]{1};
            var content = File.ReadAllText("OwOWorldSafeFile.json");
            var deserialized= JsonConvert.DeserializeObject<int[]>(content);
            return deserialized;
        }

        public static void SaveOwOWorldSafeFile(int[] safeFile)
        {
            var jsonString = JsonConvert.SerializeObject(safeFile);
            File.WriteAllText("OwOWorldSafeFile.json", jsonString);
        }
    }
}
