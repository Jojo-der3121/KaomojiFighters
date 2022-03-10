﻿using KaomojiFighters.Mobs;
using KaomojiFighters.Mobs.PlayerComponents;
using Nez;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;

namespace KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents
{
    class Player1BattleHUD : Component, ITelegramReceiver
    {
        private BattleHUD HUD;
        private Entity player;
        private Stats stats;

        public void MessageReceived(Telegram message)
        {
            if (message.Head == "its your turn")
            {
                HUD.Enabled = true;
                HUD.selectionButton.Enabled = true;
                HUD.AttackButton.Enabled = true;
                HUD.ItemButton.Enabled = true;
                HUD.SaturdayButton.Enabled = true;
            }

            if(message.Head == "its not your turn")
            {
                HUD.selectionButton.Enabled = false;
                HUD.AttackButton.Enabled = false;
                HUD.ItemButton.Enabled = false;
                HUD.SaturdayButton.Enabled = false;
                HUD.Enabled = false;
            }
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            player = Entity.Scene.FindEntity("Kaomoji01");
            TelegramService.Register(this, player.Name);
            stats = player.GetComponent<Stats>();
            HUD = Entity.AddComponent(new BattleHUD() { stats = stats, player = player });
            HUD.selectionButton.Enabled = false;
            HUD.AttackButton.Enabled = false;
            HUD.ItemButton.Enabled = false;
            HUD.SaturdayButton.Enabled = false;
            HUD.Enabled = false;
        }
        
    }
}
