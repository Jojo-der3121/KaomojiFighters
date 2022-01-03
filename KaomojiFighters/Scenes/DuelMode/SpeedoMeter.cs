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
    class SpeedoMeter : Component, IUpdatable
    {
        public List<Entity> EntityList;
        private Entity TurnPlayer;
        public bool LastPlayerFinished = true;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            TurnPlayer = EntityList[0]; // Wähle iwenn als erstes (es ist egal)
        }
        public void Update()
        {
            if (TurnPlayer.GetComponent<Stats>().ItsMyTurn == true) return; //falls der Zugspieler noch dran ist warte bis er fertig ist
            TurnPlayer.GetComponent<Stats>().Speed -= 10; //senke die Geschwindigkeit des Zugspielers, damit die Langsamereren dran kommen können



            // suche die Momentan schnellste Entity und mach sie zum TurnPlayer
            for (var i = 0; i < EntityList.Count - 1; i++)
            {
                if (EntityList[i].GetComponent<Stats>().Speed < TurnPlayer.GetComponent<Stats>().Speed)
                {
                    TurnPlayer = EntityList[i];
                }
            }
            TurnPlayer.GetComponent<Stats>().ItsMyTurn = true;


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
                if (element.GetComponent<Stats>().Speed >= 0) return false;
            }
            return true;
        }
    }
}
