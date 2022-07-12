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
        private Stats opponentStats;
        private float playerMaxHealth;
        private float opponentMaxHealth;
        private Sprite VSSinge;
        private SpriteRenderer selectionButton;
        private SpeechBubble attackButton;
        private SpeechBubble itemButton;
        private SpeechBubble saturdayButton;
        public Sprite EnergyStar;
        private VirtualButton Left;
        private VirtualButton Right;
        private VirtualButton Enter;
        private int selectionDestination;
        private Player playerComponent;
        private ItemMenu ItemMenuComponent;
        private AttackMenu AttackMenuComponent;
        private Sprite BatterieHPBar;
        private readonly List<word> Deck = new List<word>();
        public readonly List<word> Hand = new List<word>();
        public readonly List<word> GY = new List<word>();
        private const int selectionDestinationOffset = 350;
       


        public override void OnAddedToEntity()
        {
            //gets stat values for HP BAR
            playerComponent = Entity.Scene.FindEntity("Kaomoji01").GetComponent<Player>();
           
            opponentStats = Entity.Scene.FindEntity("Kaomoji02").GetComponent<Mob>().stat;
            playerMaxHealth = playerComponent.stat.HP;
            opponentMaxHealth = opponentStats.HP;

            TelegramService.Register(this, playerComponent.Entity.Name);// registers in Telegram Service with Playername

            // Adds the executional components for the MenuselectionOptions
            
            AttackMenuComponent = Entity.AddComponent(new AttackMenu() { player = playerComponent, hud = this });
            ItemMenuComponent = Entity.AddComponent(new ItemMenu());

            // Loads Sprites
            VSSinge = new Sprite(Entity.Scene.Content.LoadTexture("VS"));
             BatterieHPBar = new Sprite(Entity.Scene.Content.LoadTexture("BatterieHPBar"));
            EnergyStar = new Sprite(Entity.Scene.Content.LoadTexture("CostStar"));

            // defines MenuControlls
            Left = new VirtualButton().AddKeyboardKey(Keys.A);
            Right = new VirtualButton().AddKeyboardKey(Keys.D);
            Enter = new VirtualButton().AddKeyboardKey(Keys.Space);

            // defines selection Button
            selectionButton = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("SelectionKaoButton")));
            selectionButton.Size = new Vector2(selectionButton.Width * 3, selectionButton.Height * 3);
            selectionButton.RenderLayer = -1;
            selectionButton.LayerDepth = 0;
            selectionButton.Transform.Position = new Vector2(selectionButton.Width * 1.5f, 860 - 112 + selectionButton.Height * 1.5f);
            selectionDestination = 460;

            attackButton = new SpeechBubble(new Vector2(selectionDestination + selectionButton.Width * 1.5f, 840 ), "Attack", new Vector2(280, 165), true, 5);
            itemButton = new SpeechBubble(new Vector2(selectionDestination + selectionDestinationOffset + selectionButton.Width * 1.5f, 840), "Item", new Vector2(280, 165), true,5);
            saturdayButton = new SpeechBubble(new Vector2(selectionDestination+ selectionDestinationOffset*2 + selectionButton.Width * 1.5f, 840), "Samstag", new Vector2(280, 165), true,5);


            //Disable because not used yet
            selectionButton.Enabled = false;
            AttackMenuComponent.Enabled = false;
            ItemMenuComponent.Enabled = false;

            // loads assets and gets Data for aufzieh and ablage Stapl
            Deck.AddRange(playerComponent.stat.wordList);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;


        protected override void Render(Batcher batcher, Camera camera)
        {
            // draws Healthbars
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 700 - 20, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X + 20, 146, 700, 55), Color.Red);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X - 700 - 20 + (int)(700 * (1 - playerComponent.stat.HP / playerMaxHealth)), 146, (int)(700 * playerComponent.stat.HP / playerMaxHealth) + 1, 55), Color.DarkGreen);
            batcher.DrawRect(new Rectangle((int)Screen.Center.X + 20, 146, (int)(700 * opponentStats.HP / opponentMaxHealth), 55), Color.DarkGreen);
            batcher.Draw(BatterieHPBar, new Vector2((int)Screen.Center.X - 700 - 20 - 25, 146 - 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            batcher.Draw(BatterieHPBar, new Vector2((int)Screen.Center.X + 700 + 45, 146 - 10 + 75), Color.White, 3.14f, Vector2.Zero, 1, SpriteEffects.None, 0);
            //draws VSSing
            batcher.Draw(VSSinge, new Vector2(Screen.Center.X - VSSinge.Center.X * 0.5f - 20, 125), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            //draws ablage-/aufzieh- Stappel 
            batcher.DrawString(Graphics.Instance.BitmapFont, Deck.Count.ToString(), new Vector2(85, 1020 - 117), Color.DarkGreen, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);
            batcher.DrawString(Graphics.Instance.BitmapFont, GY.Count.ToString(), new Vector2(1920 - 130, 1020 - 117), Color.DarkGreen, 0f, Vector2.Zero, 7f, SpriteEffects.None, 0f);

            //draws MenuOptions
            if (selectionButton.Enabled)
            {
                attackButton.DrawTextField(batcher);
                itemButton.DrawTextField(batcher);
                saturdayButton.DrawTextField(batcher);
            }

            // draws energy Bar
            for ( var i = 1; i <= playerComponent.stat.energy; i++) 
            {
                batcher.Draw(EnergyStar, new Rectangle(85, 1080 - 290 - i*40,45,45), Color.CornflowerBlue);
                batcher.DrawString(Graphics.Instance.BitmapFont,  i.ToString(), new Vector2(75, 1080 - 280 - i * 40), Color.CornflowerBlue, 0f, Vector2.Zero, 3f, SpriteEffects.None,0f);
            }
        }

        public void Update()
        {
            // allows the selection Button to move and choses an Option if Space is pressed

            if (selectionDestination - selectionDestinationOffset >= (int)Screen.Center.X - selectionDestinationOffset - 150 && Left.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerComponent.stat.HP > 0)
            {
                selectionDestination -= selectionDestinationOffset;
            }
            if (selectionDestination + selectionDestinationOffset <= (int)Screen.Center.X + selectionDestinationOffset - 150 && Right.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerComponent.stat.HP > 0)
            {
                selectionDestination += selectionDestinationOffset;
            }
            selectionButton.LocalOffset = Vector2.Lerp(selectionButton.LocalOffset, new Vector2(selectionDestination, selectionButton.LocalOffset.Y), 0.06f);
            bool ignoreAttackUpdate = false;
            bool ignoreItemUpdate = false;
            if (Enter.IsPressed && !AttackMenuComponent.Enabled && !ItemMenuComponent.Enabled && selectionButton.Enabled && playerComponent.stat.HP > 0)
            {
                switch (selectionDestination)
                {
                    case 460:
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
                    case 460+ selectionDestinationOffset:
                        ItemMenuComponent.Enabled = true;
                        ignoreItemUpdate = true;
                        break;
                    case 460+ selectionDestinationOffset*2:
                        playerComponent.stat.HP = 0;
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

            if (Deck.Count != 0 || GY.Count != playerComponent.stat.wordList.Count) return;
            Deck.AddRange(GY);
            GY.Clear();


        }
        // checks received telegrams to know if its the players turn

        public void MessageReceived(Telegram message)
        {
            switch (message.Head)
            {
                case "its your turn":
                    selectionButton.Enabled = true;
                    break;
                case "its not your turn":
                    selectionButton.Enabled = false;
                    break;
            }
        }
    }
}
