using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Scenes.DuelMode
{
    class SpeedoMeter : SceneComponent, IUpdatable
    {
        public List<Stats> EntityList;
        private Stats TurnPlayer;
        public bool LastPlayerFinished = true;

        public SpeedoMeter()
        {
            EntityList = new List<Stats>();
            TurnPlayer = new Stats() { HP = 35, AttackValue = 3, Speed = 0};
        }
        
        public void Update()
        {
            if (TurnPlayer.ItsMyTurn == true) return; //falls der Zugspieler noch dran ist warte bis er fertig ist
            TurnPlayer.Speed -= 10; //senke die Geschwindigkeit des Zugspielers, damit die Langsamereren dran kommen können



            // suche die Momentan schnellste Entity und mach sie zum TurnPlayer
            for (var i = 0; i < EntityList.Count - 1; i++)
            {
                if (EntityList[i].Speed >= TurnPlayer.Speed)
                {
                    TurnPlayer = EntityList[i];
                }
            }
            TurnPlayer.ItsMyTurn = true;


            //Überprüfe ob jeder dran war und falls ja erhöhe den Speed von allen wieder auf den "Startwert"
            //if (HadEveryoneTheChanceToDoSomething())
            //{
            //    foreach (var element in EntityList)
            //    {
            //        element.GetComponent<Stats>().Speed += 10;
            //    }
            //}
        }

        private bool HadEveryoneTheChanceToDoSomething()
        {
            foreach (var element in EntityList)
            {
                if (element.Speed >= 0) return false;
            }
            return true;
        }
    }
}
