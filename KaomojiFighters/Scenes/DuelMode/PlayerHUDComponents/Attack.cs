using KaomojiFighters.Enums;
using KaomojiFighters.Scenes.DuelMode;
using KaomojiFighters.Scenes.DuelMode.PlayerHUDComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;

namespace KaomojiFighters.Mobs.PlayerComponents.PlayerHUDComponents
{
    
    class EnemyTextAttack : RenderableComponent, IUpdatable
    {

        public List<word> Deck = new List<word>();
        [Inspectable]
        public List<word> Hand = new List<word>();
        public List<word> GY = new List<word>();
        public List<int> IndexOfAllreadyUsedCards = new List<int>();
        private List<word> sentenceWords;
        private AttackMenu attackMenu;
        private word attackSentenceEnd;
        protected Scene scene;
        public BoxCollider collider;
        public Entity attackTarget;
        public SpriteRenderer EnemySprite;
        protected SpriteRenderer EntitySprite;
        protected Stats stat;
        protected AttackState oldAttackState;
        protected AttackState attackState;
        protected int Lammarsch;
        private MobHitCalculation MyAutsch;
        protected Vector2 OriginalPosition;
        protected Texture2D Bubble;


        public void enableAttack() => attackState = AttackState.approaching;


        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;



        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            scene = new Scene();
            EntitySprite = Entity.GetComponent<SpriteRenderer>();
            stat = Entity.GetComponent<Mob>().stat;
            collider = Entity.AddComponent(new BoxCollider(75, 75));
            collider.Enabled = false;
            EnemySprite = attackTarget.GetComponent<SpriteRenderer>();
            Lammarsch = Math.Sign(Entity.Position.X - attackTarget.Position.X);
            MyAutsch = Entity.GetComponent<MobHitCalculation>();
            OriginalPosition = Entity.Position;
            Bubble = Entity.Scene.Content.LoadTexture("SpeachBubble");
            Deck = new List<word>();
            Deck.AddRange(Entity.GetComponent<Mob>().stat.wordList);
            sentenceWords = new List<word>();
            attackMenu = new AttackMenu();
        }
        protected float GetAttackX() => -Lammarsch * MyAutsch.HitBox.Width / 2;

        public void Update() => attack();

        protected float EnemyXPosition() => attackTarget.Position.X + Lammarsch * (+EnemySprite.Width / 2 + EntitySprite.Width / 2 + 10);

        protected  void attack()
        {
            if (Deck.Count == 0)
            {
                Deck.AddRange(GY);
                GY.Clear();
            }

            // draw
            if (attackState == AttackState.approaching)
            {
                if (Deck.Count >= 5 && oldAttackState != AttackState.approaching)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var r = Nez.Random.NextInt(Deck.Count);
                        Hand.Add(Deck[r]);
                        Deck.RemoveAt(r);
                    }
                }
                else if (oldAttackState != AttackState.approaching)
                {
                    Hand.AddRange(Deck);
                    Deck.Clear();
                }

                var nextValidWord = GetNextValidWord();
                if (nextValidWord == null)
                {
                    attackState = AttackState.attacking;
                }
                else
                {
                    sentenceWords.Add(nextValidWord);
                }
            }
            // mainPhase1
            if (attackState == AttackState.attacking && oldAttackState != AttackState.attacking)
            {
                foreach (var element in sentenceWords)
                {
                    element.ExecuteEffect(stat);
                }

                if (sentenceWords.Count == 0 && attackSentenceEnd == null)
                {
                    stat.HP -= 3;
                    GY.AddRange(Hand);
                    Hand.Clear();
                    sentenceWords.Clear();
                    IndexOfAllreadyUsedCards.Clear();
                    TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                    TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));
                }

                if (sentenceWords.Count > 0 && sentenceWords[sentenceWords.Count - 1].typeOfWord != wordType.Nomen)
                {
                    sentenceWords.Add(attackSentenceEnd);
                    attackSentenceEnd?.ExecuteEffect(stat);
                    stat.energy -= attackSentenceEnd.cost;
                }
                attackSentenceEnd = null;
                if (sentenceWords.Count > 0)
                {
                    Core.Schedule(1.3f, (x) => attackState = AttackState.returning);
                }
                else
                {
                    Core.Schedule(1.3f, (x) => attackState = AttackState.waiting);
                }

            }
            // BattlePhase
            if (attackState == AttackState.returning && oldAttackState != AttackState.returning)
            {
            
                TelegramService.SendPrivate(new Telegram(Entity.Name, attackTarget.Name, "auf die Fresse", "tach3tach3tach3"));
                Core.Schedule(2f, (x) => attackState = AttackState.waiting);
            }
            //EndPhase
            if (attackState == AttackState.waiting && oldAttackState != AttackState.waiting)
            {
                GY.AddRange(Hand);
                Hand.Clear();
                sentenceWords.Clear();
                IndexOfAllreadyUsedCards.Clear();
                TelegramService.SendPrivate(new Telegram(Entity.Name, "SpeedoMeter", "I end my turn", "tach3tach3tach3"));
                TelegramService.SendPrivate(new Telegram(Entity.Name, Entity.Name, "its not your turn", "tach3tach3tach3"));
            }
            oldAttackState = attackState;
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            if (attackState == AttackState.returning)
            {
                var str = attackMenu.GetString(sentenceWords);
                batcher.Draw(Bubble, new Rectangle((int)Screen.Center.X + 140, 290,(int) Graphics.Instance.BitmapFont.MeasureString(str ).X *3+ 20, 50)) ;
                batcher.DrawString(Graphics.Instance.BitmapFont, str, new Vector2(Screen.Center.X + 150, 300), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

            }
        }

        private word GetNextValidWord()
        {
            for (var e = 0; e < Hand.Count; e++)
            {

                if (attackSentenceEnd == null && Hand[e].typeOfWord == wordType.Nomen)
                {
                    attackSentenceEnd = Hand[e];
                    IndexOfAllreadyUsedCards.Add(e);
                }
                foreach (var element in Hand[e].allowedPreviouseWords)
                {
                    if (sentenceWords.Count == 0 && wordType.nothing == element && stat.energy - Hand[e].cost >= 0 && NotUsedAllready(e))
                    {
                        IndexOfAllreadyUsedCards.Add(e);
                        stat.energy -= Hand[e].cost;
                        return Hand[e];
                    }
                    if (sentenceWords.Count > 0 && element == sentenceWords[sentenceWords.Count - 1].typeOfWord && NotUsedAllready(e))
                    {
                        if ((!(Hand[e].typeOfWord == wordType.Verb) && stat.energy - Hand[e].cost >= 0) || (Hand[e].typeOfWord == wordType.Verb && stat.energy - (Hand[e].cost + attackSentenceEnd.cost) >= 0))
                        {
                            IndexOfAllreadyUsedCards.Add(e);
                            stat.energy -= Hand[e].cost;
                            return Hand[e];
                        }
                    }
                }
            }

            return null;
        }

        private bool NotUsedAllready(int e)
        {
            foreach (var element in IndexOfAllreadyUsedCards)
            {
                if (element == e) return false;
            }
            return true;
        }
    }


}
