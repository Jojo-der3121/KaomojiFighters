using KaomojiFighters.Mobs;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace KaomojiFighters.Scenes.DuelMode
{
    class HUD : RenderableComponent, IUpdatable, ITelegramReceiver
    {
        private Stats playerStats;
        private Stats opponentStats;
        private float playerMaxHealth;
        private float opponentMaxHealth;
        private Sprite VSSinge;
        public SpriteRenderer selectionButton;
        private Sprite AttackButton;
        private Sprite ItemButton;
        private Sprite SaturdayButton;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private int selectionDestination;
        private Attack AttackComponent;
        private ItemMenu ItemMenuComponent;
     

        public override void OnAddedToEntity()
        {
            //gets stat values for HP BAR
            playerStats = Entity.Scene.FindEntity("Kaomoji01").GetComponent<Stats>();
            opponentStats = Entity.Scene.FindEntity("Kaomoji02").GetComponent<Stats>();
            playerMaxHealth = playerStats.HP;
            opponentMaxHealth = opponentStats.HP;
            
            TelegramService.Register(this, playerStats.Entity.Name);// registers in Telegram Service with Playername

            // Adds the executional components for the MenuselectionOptions
            AttackComponent = playerStats.Entity.GetComponent<Attack>();
            ItemMenuComponent = Entity.AddComponent(new ItemMenu() { playerEntity = playerStats.Entity });

            // Loads Sprites
            VSSinge = new Sprite(Entity.Scene.Content.LoadTexture("VS"));
            AttackButton = new Sprite(Entity.Scene.Content.LoadTexture("AttackKaoButton"));
            ItemButton = new Sprite(Entity.Scene.Content.LoadTexture("ItemKaoButton"));
            SaturdayButton = new Sprite(Entity.Scene.Content.LoadTexture("SamstagKaoButton"));

            // defines MenuControlls
            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);

            // defines selection Button
            selectionButton = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(selectionButton.Width * 3, selectionButton.Height * 3);
            selectionButton.RenderLayer = -1;
            selectionButton.LayerDepth = 0;
            selectionButton.Transform.Position = new Vector2((int)Screen.Center.X - 350 - 150-460 + selectionButton.Width *1.5f, 860 - 112+ selectionButton.Height *1.5f);
            selectionDestination = 1920 / 2 - 350 - 150;

            //Disable because not used yet
            selectionButton.Enabled = false;
            AttackComponent.Enabled = false;
            ItemMenuComponent.Enabled = false;
        }
        public override RectangleF Bounds => new RectangleF(0,0,1920,1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;
            
        
        protected override void Render(Batcher batcher, Camera camera)
        {
            // draws Healthbars
            batcher.DrawRect(new Rectangle((int)Screen.Center.X-700-10, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X+10, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X-700-10+ (int)(700 *(1- playerStats.HP / playerMaxHealth)), 146,  (int)(700*playerStats.HP / playerMaxHealth) +1, 55), Color.DarkGreen);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X+10, 146, (int)(700 * opponentStats.HP / opponentMaxHealth), 55), Color.DarkGreen);
            //draws VSSing
            batcher.Draw(VSSinge, new Vector2(Screen.Center.X-VSSinge.Center.X*0.5f-20, 125),Color.White,0,Vector2.Zero,0.5f,SpriteEffects.None,0);
            if (selectionButton.Enabled)
            {
                batcher.Draw(AttackButton, new Rectangle((int)Screen.Center.X - 350 - 150 + 20, 860 - 112 + 15, 260, 185));
                batcher.Draw(ItemButton, new Rectangle((int)Screen.Center.X - 150 + 20, 860 - 112 + 15, 260, 185));
                batcher.Draw(SaturdayButton, new Rectangle((int)Screen.Center.X + 350 - 150 + 20, 860 - 112 + 15, 260, 185));
            }
            //draws the BasicMenuOption Buttons
        }

        public void Update()
        {
            // allows the selection Button to move and choses an Option if Space is pressed

            if (selectionDestination - 350 >= (int)Screen.Center.X - 350 - 150 && Left.IsPressed && !AttackComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled)
            {
                selectionDestination -= 350;
            }
            if (selectionDestination + 350 <= (int)Screen.Center.X + 350 - 150 && Right.IsPressed && !AttackComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled)
            {
                selectionDestination += 350;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);
            if (Enter.IsReleased && !AttackComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled)
            {
                switch (selectionDestination)
                {
                    case 1920/2 - 350 - 150:
                        AttackComponent.Enabled = true;
                        AttackComponent.enableAttack();
                        break;
                    case 1920 / 2  - 150:
                        ItemMenuComponent.Enabled = true;
                        ItemMenuComponent.SelectionButton.Enabled = true;
                        ItemMenuComponent.ItemMenuDisplay.Enabled = true;
                        foreach (var element in ItemMenuComponent.Textures)
                        {
                            element.Enabled = true;
                        }
                        break;
                    case 1920 / 2+ 350 - 150:
                        playerStats.HP = 0;
                        break;
                }
            }
        }
        // checks received telegrams to know if its the players turn

        public void MessageReceived(Telegram message)
        {
            if (message.Head == "its your turn")
            {
                selectionButton.Enabled = true;
            }

            if (message.Head == "its not your turn")
            {
                selectionButton.Enabled = false;
            }
        }
    }
}
