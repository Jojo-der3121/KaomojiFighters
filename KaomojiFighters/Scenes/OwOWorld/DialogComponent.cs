using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

namespace KaomojiFighters.Scenes.OwOWorld
{
    class DialogComponent : RenderableComponent, IUpdatable
    {
        private RenderableComponent OwOWorldSprite;
        private Sprite[] sprites;
        private List<Dialog> _dialog;
        private OwOWorldPlayer _player;
        private int dialogIndex;
        private VirtualButton proceed;
        private SpeechBubble bubble;
        private int WhichNPC;
        private RectangleF NPCArea;
        private int owoWorldScale;

        public DialogComponent(int whichDiaKaomoji, OwOWorldPlayer player, List<Dialog> dialog, TmxObject element, int WorldScale)
        {
            _player = player;
            owoWorldScale = WorldScale;
            _dialog = dialog;
            WhichNPC = whichDiaKaomoji;
            NPCArea = new RectangleF(element.X, element.Y, element.Width, element.Height);
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            proceed = new VirtualButton().AddKeyboardKey(Keys.Space);
            bubble = new SpeechBubble(new Vector2(Screen.Center.X, 1080 / 5 * 4), _dialog[dialogIndex].txt,
                new Vector2(1920, 1020 / 5 * 2), false, 5);
            sprites = GetSprites(WhichNPC);
            OwOWorldSprite = Entity.AddComponent(new SpriteRenderer(Entity.Scene.Content.LoadTexture("Kaomoji02")) { Size = new Vector2(310, 92) / owoWorldScale }).SetLocalOffset(new Vector2(NPCArea.X + NPCArea.Width / 2, NPCArea.Y + NPCArea.Height / 2) * owoWorldScale);

        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            OwOWorldSprite.Enabled = false;
            _player.renderer.Enabled = false;
            _player.mover.Enabled = false;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            if (OwOWorldSprite == null || _player == null) return;
            OwOWorldSprite.Enabled = true;
            _player.renderer.Enabled = true;
            _player.mover.Enabled = true;
        }

        private Sprite[] GetSprites(int i)
        {
            return new Sprite[]
            {
                new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02")),
                    new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Attack")),
                    new Sprite(Entity.Scene.Content.LoadTexture("Kaomoji02Hurt"))
            };
        }

        public void Update()
        {
            if (proceed.IsPressed)
            {
                dialogIndex++;
                bubble.GetSpeech(_dialog[dialogIndex].txt);
            }
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            bubble.DrawTextField(batcher);
            batcher.Draw(sprites[_dialog[dialogIndex].emotion],new RectangleF(1920/3-300,Screen.Center.Y,600,200));
            batcher.Draw(GetPlayerSprite(),new RectangleF(1920/3-300,Screen.Center.Y,600,200));
        }

        private Sprite GetPlayerSprite()
        {
            switch (_dialog[dialogIndex].emotionPL)
            {
                case 1:
                    return _player.stat.sprites.Normal;
                case 2:
                    return _player.stat.sprites.Hurt;
                case 3:
                    return _player.stat.sprites.Attack;
            }

            return new Sprite(Entity.Scene.Content.LoadTexture("R")) ;
        }
    }

    class Dialog
    {
        public string txt;
        public int emotion;
        public int emotionPL;

        public Dialog( string Text, int NPCEmotion, int PlayerEmotion)
        {
            txt = Text;
            emotion = NPCEmotion;
            emotionPL = PlayerEmotion;
        }
    }
}
