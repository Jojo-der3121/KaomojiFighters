using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs.PlayerComponents;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace KaomojiFighters.Mobs
{
    class Player : Component, IUpdatable
    {
        private Scene scene;
        private Stats stats;
        private BattleHUD HUD;
        public BoxCollider HitBox;
        public WASDMovement MovementComponent;
        
        public override void OnAddedToEntity()
        {
            scene = new Scene();
            base.OnAddedToEntity();
            HUD = Entity.AddComponent(new BattleHUD());
            HUD.selectionButton.Enabled = false;
            HUD.AttackButton.Enabled = false;
            HUD.ItemButton.Enabled = false;
            HUD.SaturdayButton.Enabled = false;
            HUD.Enabled = false;
            MovementComponent = Entity.AddComponent(new WASDMovement());
            MovementComponent.Enabled = false;
            Entity.AddComponent(new SpriteRenderer(scene.Content.LoadTexture("Kaomoji01")));
            stats = Entity.AddComponent(new Stats() { HP = 35, AttackValue = 30, Speed = 3 , sprites = new Enums.Sprites() {  Normal = "Kaomoji01", Attack = "Kaomoji01Attack", Hurt = "" } });
        }

        public void Update()
        {
            if (stats.ItsMyTurn && !HUD.Enabled)
            {
                HUD.Enabled = true;
                HUD.selectionButton.Enabled = true;
                HUD.AttackButton.Enabled = true;
                HUD.ItemButton.Enabled = true;
                HUD.SaturdayButton.Enabled = true;
            }
            if (!stats.ItsMyTurn && HUD.Enabled)
            {
                HUD.selectionButton.Enabled = false;
                HUD.AttackButton.Enabled = false;
                HUD.ItemButton.Enabled = false;
                HUD.SaturdayButton.Enabled = false;
                HUD.Enabled = false;
            }
        }
    }
}
