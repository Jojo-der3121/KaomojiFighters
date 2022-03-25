using KaomojiFighters.Enums;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs
{

    abstract class word: Component
    {
        public string actualWord;
        public string sensitivTopic;
        public wordType typeOfWord;
        public List<wordType> allowedPreviouseWords;
        public Stats stats;
        public int cost;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            stats = Entity.GetComponent<Stats>();
            CalibratePerameters();
        }

        public abstract void CalibratePerameters();

        public virtual bool IsUsable(wordType previouseWord)
        {
            foreach(var allowedWord in allowedPreviouseWords)
            {
                if(allowedWord == previouseWord)
                {
                    return true;
                }
            }
            stats.HP -= 3;
            return false;
        }

        public abstract void wordEffekt();
    }

    class YourMom : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "Your Mom";
            sensitivTopic = "Mom Jokes";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
            cost = 3;
        }

        public override void wordEffekt() => stats.AttackValue -= 3;
        
    }

    class And : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "and";
            sensitivTopic = "none";
            typeOfWord = wordType.Konjunktion;
            allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Verb };
            cost = 1;
        }
        public override void wordEffekt()
        {
            stats.energy += 1;
            stats.Defence += 2; 
        }
    }

    class AFishHead : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "a Fish head";
            sensitivTopic = "appearance";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
            cost = 5;
        }
        public override void wordEffekt () =>   stats.Defence ++;
    }

    class DogFood : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "Dog Food";
            sensitivTopic = "none";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Verb, wordType.Konjunktion };
            cost = 5;
        }
        public override void wordEffekt() => stats.HP+= 3;
    }

    class I : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "I";
            sensitivTopic = "none";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion };
            cost = 2;
        }

        public override void wordEffekt() => stats.Defence+= 1;
    }
    class You : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "You";
            sensitivTopic = "insecure";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion, wordType.Verb };
            cost = 2;
        }

        public override void wordEffekt() => stats.AttackValue += 1;
    }
    class Legos : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "Legos";
            sensitivTopic = "ptsd";
            typeOfWord = wordType.Nomen;
            allowedPreviouseWords = new List<wordType> { wordType.nothing, wordType.Konjunktion, wordType.Verb };
            cost = 2;
        }

        public override void wordEffekt() => stats.AttackValue += 10;
    }
    class StepOn : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "step on";
            sensitivTopic = "clumsy";
            typeOfWord = wordType.Verb;
            allowedPreviouseWords = new List<wordType> {  wordType.Konjunktion, wordType.Nomen };
            cost = 2;
        }

        public override void wordEffekt() => stats.Speed += 0; // fix later (3)
    }
    class Hope : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "hope";
            sensitivTopic = "insecure";
            typeOfWord = wordType.Verb;
            allowedPreviouseWords = new List<wordType> { wordType.Konjunktion, wordType.Nomen };
            cost = 2;
        }

        public override void wordEffekt() => stats.HP += 3;
    }

    class ducked : word
    {
        public override void CalibratePerameters()
        {
            actualWord = "ducked";
            sensitivTopic = "honor";
            typeOfWord = wordType.Verb;
            allowedPreviouseWords = new List<wordType> { wordType.Nomen, wordType.Konjunktion };
            cost = 5;
        }

        public override void wordEffekt() => stats.AttackValue += 5;
    }
}
