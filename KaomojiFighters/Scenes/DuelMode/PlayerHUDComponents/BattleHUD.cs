using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaomojiFighters.Mobs.PlayerComponents
{
    class BattleHUD : Component, IUpdatable
    {
        private Scene scene;
        private Attack AttackComponent;
        public Stats stats;
        public Entity player;
        private ItemMenu ItemMenuComponent;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        public SpriteRenderer selectionButton;
        public SpriteRenderer AttackButton;
        public SpriteRenderer ItemButton;
        public SpriteRenderer SaturdayButton;
        private int selectionDestination;
        public int selectedButton = 0;

        public override void Initialize()
        {
            base.Initialize();
            scene = new Scene();
            selectionButton = new SpriteRenderer(scene.Content.LoadTexture("SelectionKaoButton"));
            AttackButton = new SpriteRenderer(scene.Content.LoadTexture("AttackKaoButton"));
            ItemButton = new SpriteRenderer(scene.Content.LoadTexture("ItemKaoButton"));
            SaturdayButton = new SpriteRenderer(scene.Content.LoadTexture("SamstagKaoButton"));
            AttackComponent = player.AddComponent(new s1() { attackTarget = Entity.Scene.FindEntity("Kaomoji02") });
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            ItemMenuComponent = Entity.AddComponent(new ItemMenu() {playerEntity = player });
            AttackComponent.Enabled = false;
            ItemMenuComponent.Enabled = false;

            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);

            selectionButton = Entity.AddComponent(selectionButton);
            selectionButton.Size = new Vector2(selectionButton.Width * 3, selectionButton.Height * 3);
            selectionButton.RenderLayer = 0;
            selectionButton.LayerDepth = 0;
            selectionButton.LocalOffset = new Vector2(1920 / 4-350 +600, 1080 / 2 - 375+700);
            selectionDestination = 1920 / 4 - 350 + 600;

            AttackButton = Entity.AddComponent(AttackButton);
            AttackButton.Size = new Vector2(AttackButton.Width * 2.7f, AttackButton.Height * 2.5f);
            AttackButton.RenderLayer = 0;
            AttackButton.LayerDepth = 0.1f;
            AttackButton.LocalOffset = new Vector2(1920 / 4 - 350 + 600, 1080 / 2 - 380 + 700);

            ItemButton = Entity.AddComponent(ItemButton);
            ItemButton.Size = new Vector2(ItemButton.Width * 2.7f, ItemButton.Height * 2.5f);
            ItemButton.RenderLayer = 0;
            ItemButton.LayerDepth = 0.1f;
            ItemButton.LocalOffset = new Vector2(1920 / 4+ 600, 1080 / 2 - 380 + 700);

            SaturdayButton = Entity.AddComponent(SaturdayButton);
            SaturdayButton.Size = new Vector2(SaturdayButton.Width * 2.7f, SaturdayButton.Height * 2.5f);
            SaturdayButton.RenderLayer = 0;
            SaturdayButton.LayerDepth = 0.1f;
            SaturdayButton.LocalOffset = new Vector2(1920 / 4 + 350 + 600, 1080 / 2 - 380 + 700);
        }

        public void Update()
        {


            if (selectionDestination - 350 >= 1920 / 4 - 350 + 600 && Left.IsPressed && !AttackComponent.Enabled && !ItemMenuComponent.Enabled)
            {
                selectionDestination -= 350;
            }
            if (selectionDestination + 350 <= 1920 / 4 + 350 + 600 && Right.IsPressed && !AttackComponent.Enabled && !ItemMenuComponent.Enabled)
            {
                selectionDestination += 350;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);
            if (Enter.IsReleased && !AttackComponent.Enabled && !ItemMenuComponent.Enabled)
            {
                switch (selectionDestination)
                {
                    case 1920 / 4 - 350 + 600:
                        AttackComponent.Enabled = true;
                        break;
                    case 1920 / 4 + 600:
                        ItemMenuComponent.Enabled = true;
                        ItemMenuComponent.SelectionButton.Enabled = true;
                        ItemMenuComponent.ItemMenuDisplay.Enabled = true;
                        foreach (var element in ItemMenuComponent.Textures)
                        {
                            element.Enabled = true;
                        }
                        break;
                    case 1920 / 4 + 350 + 600:
                        stats.HP = 0;
                        break;
                }
            }

        }
    }
}
