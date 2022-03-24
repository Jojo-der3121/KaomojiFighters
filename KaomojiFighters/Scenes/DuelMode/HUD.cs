using KaomojiFighters.Mobs;
using KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents;
using KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System.Collections.Generic;

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
        private Player playerComponent;
        private ItemMenu ItemMenuComponent;
        private AttackMenu AttackMenuComponent;
        private Sprite BatterieHPBar;
        private Texture2D deckTexture;
        public List<word> Deck = new List<word>();
        public List<word> Hand = new List<word>();
        public List<word> GY = new List<word>();


        public override void OnAddedToEntity()
        {
            //gets stat values for HP BAR
            playerStats = Entity.Scene.FindEntity("Kaomoji01").GetComponent<Stats>();
            opponentStats = Entity.Scene.FindEntity("Kaomoji02").GetComponent<Stats>();
            playerMaxHealth = playerStats.HP;
            opponentMaxHealth = opponentStats.HP;

            TelegramService.Register(this, playerStats.Entity.Name);// registers in Telegram Service with Playername

            // Adds the executional components for the MenuselectionOptions
            playerComponent = playerStats.Entity.GetComponent<Player>();
            AttackMenuComponent = Entity.AddComponent(new AttackMenu() { player = playerComponent, hud = this });
            ItemMenuComponent = Entity.AddComponent(new ItemMenu() { playerEntity = playerStats.Entity });

            // Loads Sprites
            VSSinge = new Sprite(Entity.Scene.Content.LoadTexture("VS"));
            AttackButton = new Sprite(Entity.Scene.Content.LoadTexture("AttackKaoButton"));
            ItemButton = new Sprite(Entity.Scene.Content.LoadTexture("ItemKaoButton"));
            SaturdayButton = new Sprite(Entity.Scene.Content.LoadTexture("SamstagKaoButton"));
            BatterieHPBar = new Sprite(Entity.Scene.Content.LoadTexture("BatterieHPBar"));

            // defines MenuControlls
            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);

            // defines selection Button
            selectionButton = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(selectionButton.Width * 3, selectionButton.Height * 3);
            selectionButton.RenderLayer = -1;
            selectionButton.LayerDepth = 0;
            selectionButton.Transform.Position = new Vector2((int)Screen.Center.X - 350 - 150 - 460 + selectionButton.Width * 1.5f, 860 - 112 + selectionButton.Height * 1.5f);
            selectionDestination = 1920 / 2 - 350 - 150;

            //Disable because not used yet
            selectionButton.Enabled = false;
            AttackMenuComponent.Enabled = false;
            ItemMenuComponent.Enabled = false;

            // loads assets and gets Data for aufzieh and ablage Stapl
            deckTexture = Entity.Scene.Content.LoadTexture("AttackOptions");
            Deck.AddRange(playerComponent.WordList);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;


        protected override void Render(Batcher batcher, Camera camera)
        {
            // draws Healthbars
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 700 - 20, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X + 20, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 700 - 20 + (int)(700 * (1 - playerStats.HP / playerMaxHealth)), 146, (int)(700 * playerStats.HP / playerMaxHealth) + 1, 55), Color.DarkGreen);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X + 20, 146, (int)(700 * opponentStats.HP / opponentMaxHealth), 55), Color.DarkGreen);
            batcher.Draw(BatterieHPBar, new Vector2((int)Screen.Center.X - 700 - 20 - 25, 146 - 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            batcher.Draw(BatterieHPBar, new Vector2((int)Screen.Center.X + 700 + 45, 146 - 10 + 75), Color.White, 3.14f, Vector2.Zero, 1, SpriteEffects.None, 0);
            //draws VSSing
            batcher.Draw(VSSinge, new Vector2(Screen.Center.X - VSSinge.Center.X * 0.5f - 20, 125), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            //draws ablage-/aufzieh- Stappel 
            batcher.Draw(deckTexture, new Rectangle(20, 1020 - 175, 150, 150));
            batcher.Draw(deckTexture, new Rectangle(1920 - 170, 1020 - 175, 150, 150));
            batcher.DrawString(Graphics.Instance.BitmapFont, Deck.Count.ToString(), new Vector2(35, 1020 - 150), Color.Black, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, GY.Count.ToString(), new Vector2(1920 - 145, 1020 - 150), Color.Black, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);

            //draws MenuOptions
            if (selectionButton.Enabled)
            {
                batcher.Draw(AttackButton, new Rectangle((int)Screen.Center.X - 350 - 150 + 20, 860 - 112 + 15, 260, 185));
                batcher.Draw(ItemButton, new Rectangle((int)Screen.Center.X - 150 + 20, 860 - 112 + 15, 260, 185));
                batcher.Draw(SaturdayButton, new Rectangle((int)Screen.Center.X + 350 - 150 + 20, 860 - 112 + 15, 260, 185));
            }
        }

        public void Update()
        {
            // allows the selection Button to move and choses an Option if Space is pressed

            if (selectionDestination - 350 >= (int)Screen.Center.X - 350 - 150 && Left.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerStats.HP > 0)
            {
                selectionDestination -= 350;
            }
            if (selectionDestination + 350 <= (int)Screen.Center.X + 350 - 150 && Right.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerStats.HP > 0)
            {
                selectionDestination += 350;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);
            bool ignoreAttackUpdate = false;
            bool ignoreItemUpdate = false;
            if (Enter.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerStats.HP > 0)
            {
                switch (selectionDestination)
                {
                    case 1920 / 2 - 350 - 150:
                        if (Hand.Count == 0)
                        {
                            if (Deck.Count >= 5)
                            {
                                for (var i = 0; i < 5; i++)
                                {
                                    var r = Random.NextInt(Deck.Count);
                                    Hand.Add(Deck[r]);
                                    Deck.RemoveAt(r);
                                }
                            }
                            else
                            {
                                Hand.AddRange(Deck);
                                Deck.Clear();
                            }
                        }

                        AttackMenuComponent.Enabled = true;
                        ignoreAttackUpdate = true;
                        break;
                    case 1920 / 2 - 150:
                        ItemMenuComponent.Enabled = true;
                        ItemMenuComponent.SelectionButton.Enabled = true;
                        ItemMenuComponent.ItemMenuDisplay.Enabled = true;
                        foreach (var element in ItemMenuComponent.Textures)
                        {
                            element.Enabled = true;
                        }
                        ignoreItemUpdate = true;
                        break;
                    case 1920 / 2 + 350 - 150:
                        playerStats.HP = 0;
                        break;
                }
            }
            if (AttackMenuComponent.Enabled && !ignoreAttackUpdate)
            {
                AttackMenuComponent.Update();
            }
            if (ItemMenuComponent.Enabled && !ignoreItemUpdate)
            {
                ItemMenuComponent.Update();
            }
            if (Deck.Count == 0 && GY.Count == playerComponent.WordList.Count)
            {
                Deck.AddRange(GY);
                GY.Clear();
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
