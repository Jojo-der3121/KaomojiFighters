using KaomojiFighters.Mobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters
{
    class JsonConverter
    {
        public static Stats Deserialize(string filePath)
        {
            var content = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Stats>(content);
        }

        public static void Serialize(object obj, string filePath)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
