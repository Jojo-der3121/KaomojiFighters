using KaomojiFighters.Mobs;
using Newtonsoft.Json;
using System.IO;
using KaomojiFighters.Objects;

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
            return deserializedStats;
        }

        public static void SaveStats(Stats stats)
        {
            var jsonString = JsonConvert.SerializeObject(stats);
            File.WriteAllText("PlayerStats.json", jsonString);
        }
    }
}
