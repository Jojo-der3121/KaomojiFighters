using KaomojiFighters.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace KaomojiFighters.Mobs
{

    public enum Wort
    {
        YourMom, And, AFishHead, DogFood, I, You, Legos, StepOn, Hope, Fucked, But, AButtfan, Sused, Killed, APuppy, Insulted,
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
                case Wort.But:
                    actualWord = "but";
                    sensitivTopic = "none";
                    description = "increases ATK by 3";
                    typeOfWord = wordType.Konjunktion;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.nothing };
                    cost = 1;
                    wordEffect = (x) => x.AttackValue += 3;
                    break;
                case Wort.AButtfan:
                    actualWord = "a Buttfan";
                    sensitivTopic = "appearance";
                    description = "increases DEF by 3";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.Verb, wordType.nothing, wordType.Konjunktion };
                    cost = 3;
                    wordEffect = (x) => x.Defence += 3;
                    break;
                case Wort.Sused:
                    actualWord = "sused";
                    sensitivTopic = "honor";
                    description = "increases ATK by 2";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Konjunktion };
                    cost = 2;
                    wordEffect = (x) => x.AttackValue += 2;
                    break;
                case Wort.Killed:
                    actualWord = "killed";
                    sensitivTopic = "ptsd";
                    description = "increases ATK by 15";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Konjunktion };
                    cost = 7;
                    wordEffect = (x) => x.AttackValue += 15;
                    break;
                case Wort.APuppy:
                    actualWord = "a Puppy";
                    sensitivTopic = "cute";
                    description = "recovers 12 HP";
                    typeOfWord = wordType.Nomen;
                    allowedPreviouseWords = new List<wordType> { wordType.Verb, wordType.nothing, wordType.Konjunktion };
                    cost = 8;
                    wordEffect = (x) => x.HP += 12;
                    break;
                case Wort.Insulted:
                    actualWord = "insulted";
                    sensitivTopic = "honor";
                    description = "recovers 7 HP";
                    typeOfWord = wordType.Verb;
                    allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Konjunktion };
                    cost = 4;
                    wordEffect = (x) => x.HP += 7;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ExecuteEffect(Stats stats) => wordEffect(stats);

        public static List<word> GetWordList() => new List<word>()
        {
            new word(Wort.I), new word(Wort.StepOn), new word(Wort.Legos), new word(Wort.Hope), new word(Wort.You),
            new word(Wort.Fucked), new word(Wort.YourMom), new word(Wort.And), new word(Wort.AFishHead),
            new word(Wort.DogFood)
        };

    }
}
