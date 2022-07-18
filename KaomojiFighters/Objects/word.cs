using KaomojiFighters.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaomojiFighters.Mobs
{

    public enum Wort
    {
        YourMom, And, AFishHead, DogFood, I, You, Legos, StepOn, Hope, Fucked
    }

    public class word
    {
        [JsonIgnore]
        public string actualWord;
        [JsonIgnore]
        public string sensitivTopic;
        [JsonIgnore]
        public string description;
        [JsonIgnore]
        public wordType typeOfWord;
        [JsonIgnore]
        public List<wordType> allowedPreviouseWords;
        [JsonIgnore]
        public int cost;
        public Wort Word;
        [JsonIgnore]
        public Action<Stats> wordEffect;

        public word(Wort wort)
        {
            Word = wort;
            GetRightWordProperties();
        }

        [JsonConstructor]
        public word()
        {
            
        }

        public void GetRightWordProperties()
        {

            switch (Word)
            {
                case Wort.YourMom:
                    actualWord = "Your Mom";
                    sensitivTopic = "MomJokes";
                    description = "decreases ATK by 3";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
                    cost = 3;
                    wordEffect = (x) => x.AttackValue -= 3;
                    break;
                case Wort.And:
                    actualWord = "and";
                    sensitivTopic = "none";
                    description = "increases DEF by 2";
                    typeOfWord = wordType.Konjunktion;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Verb };
                    cost = -1;
                    wordEffect = (x) => x.Defence += 2;
                    break;
                case Wort.AFishHead:
                    actualWord = "a Fish head";
                    sensitivTopic = "appearance";
                    description = "increases DEF by 1";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
                    cost = 5;
                    wordEffect = (x) => x.Defence++;
                    break;
                case Wort.DogFood:
                    actualWord = "Dog Food";
                    sensitivTopic = "none";
                    description = "recovers 3 HP";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
                    cost = 5;
                    wordEffect = (x) => x.HP += 3;
                    break;
                case Wort.I:
                    actualWord = "I";
                    sensitivTopic = "none";
                    description = "increases DEF by 1";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion };
                    cost = 2;
                    wordEffect = (x) => x.Defence += 1;
                    break;
                case Wort.You:
                    actualWord = "You";
                    sensitivTopic = "insecure";
                    description = "increases ATK by 1";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion, wordType.Verb };
                    cost = 2;
                    wordEffect = (x) => x.AttackValue += 1;
                    break;
                case Wort.Legos:
                    actualWord = "Legos";
                    sensitivTopic = "ptsd";
                    description = "increases ATK by 10";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion, wordType.Verb };
                    cost = 2;
                    wordEffect = (x) => x.AttackValue += 10;
                    break;
                case Wort.StepOn:
                    actualWord = "step on";
                    sensitivTopic = "clumsy";
                    description = " used to do stuff ^^`";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Konjunktion, wordType.Nomen };
                    cost = 2;
                    wordEffect = (x) => x.Speed += 0;
                    break;
                case Wort.Hope:
                    actualWord = "hope";
                    sensitivTopic = "insecure";
                    description = "restores 3 HP";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Konjunktion, wordType.Nomen };
                    cost = 2;
                    wordEffect = (x) => x.HP += 3;
                    break;
                case Wort.Fucked:
                    actualWord = "ducked";
                    sensitivTopic = "honor";
                    description = "increases ATK by 5";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Konjunktion };
                    cost = 5;
                    wordEffect = (x) => x.AttackValue += 5;
                    break;
            }
        }

        public void ExecuteEffect(Stats stats) => wordEffect(stats);

        //public virtual bool IsUsable(wordType previouseWord)
        //{
        //    foreach (var allowedWord in allowedPreviouseWords)
        //    {
        //        if (allowedWord == previouseWord)
        //        {
        //            return true;
        //        }
        //    }
        //    stats.HP -= 3;
        //    return false;
        //}

        public static List<word> GetWordList() => Enum.GetValues(typeof(Wort)).OfType<Wort>().Select((x) => new word(x)).ToList();

    }
}
