using System.Collections.Generic;
using KaomojiFighters.Mobs;
using Nez;
using Nez.Sprites;
using Nez.Tweens;

namespace KaomojiFighters.Scenes.DuelMode
{
    class SpeedoMeter : SceneComponent, ITelegramReceiver
    {
        public List<Mob> EntityList;
        private Mob TurnPlayer;
        private bool LastPlayerFinished = true;

        public SpeedoMeter()
        {
            EntityList = new List<Mob>();
            TelegramService.Register(this,"SpeedoMeter");
        }

        public override void Update()
        {
            base.Update();
            //checks if everyone is still alive and if not switches the scene to title screen Scene
            foreach (var battlingEntity in EntityList)
            {
                if (battlingEntity.stat.HP <= 0)
                {
                    var player = Scene.FindEntity("Kaomoji01");
                    var stats = SafeFileLoader.LoadStats();
                    if (battlingEntity.Entity != player)
                    {
                        stats.itemList = player.GetComponent<Player>().stat.itemList;
                        stats.Gold += battlingEntity.stat.Gold;
                        SafeFileLoader.SaveStats(stats);
                    }
                    var deadEntity = battlingEntity.Entity.GetComponent<SpriteRenderer>();
                    deadEntity.Entity.Tween("Rotation", 30f, 1).SetLoops(LoopType.PingPong, 1).SetLoopCompletionHandler((x)=> Core.StartSceneTransition(new FadeTransition(() => new OverworldScene(stats.OwOworldPosition)))).Start();
                    this.Enabled = false;
                    return;
                }
            }
            // suche die Momentan schnellste Entity und mach sie zum TurnPlayer

            if (!LastPlayerFinished || EntityList.Count == 0) return;
            TurnPlayer = EntityList[0];
            for (var i = 1; i <= EntityList.Count - 1; i++)
            {
                if (EntityList[i].stat.Speed >= TurnPlayer.stat.Speed)
                {
                    TurnPlayer = EntityList[i];
                }
            }
            TelegramService.SendPrivate(new Telegram("SpeedoMeter", TurnPlayer.Entity.Name, "its your turn", "tach3tach3tach3"));
            LastPlayerFinished = false;


            //Überprüfe ob jeder dran war und falls ja erhöhe den Speed von allen wieder auf den "Startwert" and also give every character 10 energy
            if (HadEveryoneTheChanceToDoSomething())
            {
                foreach (var element in EntityList)
                {
                    while (element.stat.Speed <= 0)
                    {
                        element.stat.Speed += 10;
                    }
                    element.stat.energy += 7; //achsel schmied
                }
            }


        }

        private bool HadEveryoneTheChanceToDoSomething()
        {
            foreach (var element in EntityList)
            {
                if (element.stat.Speed >= 0) return false;
            }
            return true;
        }

        public void MessageReceived(Telegram message)
        {
            if (message.Head == "I end my turn")
            {
                TurnPlayer.stat.Speed -= 10;
                LastPlayerFinished = true;
            }
        }
    }
}
