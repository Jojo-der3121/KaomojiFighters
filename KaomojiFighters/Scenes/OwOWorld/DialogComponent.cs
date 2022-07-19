using System;
using System.Collections.Generic;
using KaomojiFighters.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private VirtualButton quit;
        private SpeechBubble bubble;
        private int WhichNPC;
        private RectangleF NPCArea;
        private int owoWorldScale;
        private SoundEffect sfx1;
        private SoundEffect sfx2;
        private SoundEffect sfx3;
        private SoundEffectInstance[] sfxArray = new SoundEffectInstance[3];
        private List<SoundEffectInstance> sfxSpeech = new List<SoundEffectInstance>();
        private int soundToBePlayed;

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
            SetRenderLayer(4);
            sfx1 = Entity.Scene.Content.Load<SoundEffect>("school-fire-alarm-loud-beepflac-14807-mp3cutnet-1_bJW7loWC");
            sfx2 = Entity.Scene.Content.Load<SoundEffect>("school-fire-alarm-loud-beepflac-14807-mp3cutnet-2_UcRfjEGp2");
            sfx3 = Entity.Scene.Content.Load<SoundEffect>("school-fire-alarm-loud-beepflac-14807-mp3cutnet-4_cpLbcezA3");
            sfxArray[0] = sfx1.CreateInstance();
            sfxArray[1] = sfx2.CreateInstance();
            sfxArray[2] = sfx3.CreateInstance();
            proceed = new VirtualButton().AddKeyboardKey(Keys.Space);
            quit = new VirtualButton().AddKeyboardKey(Keys.Back);
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
            _player.Enabled = false;
            bubble.GetSpeech(_dialog[dialogIndex].txt);
            sfxSpeech.Clear();
            soundToBePlayed = 0;
            GetSFXSpeech();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            dialogIndex = 0;
            if (OwOWorldSprite == null || _player == null) return;
            OwOWorldSprite.Enabled = true;
            _player.Enabled = true;
            _player.renderer.Enabled = true;
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
            if (soundToBePlayed == 0)
            {
                sfxSpeech[soundToBePlayed].Play();
                soundToBePlayed++;
            }
            if (soundToBePlayed + 1 < sfxSpeech.Count && sfxSpeech[soundToBePlayed].State == SoundState.Stopped)
            {
                soundToBePlayed++;
                sfxSpeech[soundToBePlayed].Play();
            }

            if (proceed.IsPressed && dialogIndex >= _dialog.Count - 1)
            {
                dialogIndex = -1;
                Enabled = false;
            }
            if (proceed.IsPressed && dialogIndex < _dialog.Count - 1)
            {
                sfxSpeech.Clear();
                soundToBePlayed = 0;
                dialogIndex++;
                bubble.GetSpeech(_dialog[dialogIndex].txt);
               GetSFXSpeech();
            }
            if (quit.IsPressed)
            {
                sfxSpeech.Clear();
                Enabled = false;
            }
        }

        protected override void Render(Batcher batcher, Camera camera)
        {
            batcher.DrawRect(Vector2.Zero, 1920, 1080, new Color(31, 134, 119) * 0.5f);
            batcher.Draw(sprites[_dialog[dialogIndex].emotion - 1], new RectangleF(1920 / 5 * 4 - 300, Screen.Center.Y - 100, 600, 200));
            batcher.Draw(GetPlayerSprite(), new RectangleF(1920 / 5 - 300, Screen.Center.Y - 100, 600, 200));
            bubble.DrawTextField(batcher);
        }
        public override RectangleF Bounds => new RectangleF(0, 0, 1920, 1080);
        public override bool IsVisibleFromCamera(Camera camera) => true;

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
            return new Sprite(Entity.Scene.Content.LoadTexture("R"));
        }

        private void GetSFXSpeech()
        {
            var r = new System.Random();
            for (var i = 0; i < bubble._speech.Count; i++)
            {
                sfxSpeech.Add(sfxArray[r.Next(0, 2)]);
            }
        }
    }

    class Dialog
    {
        public string txt;
        public int emotion;
        public int emotionPL;

        public Dialog(string Text, int NPCEmotion, int PlayerEmotion)
        {
            txt = Text;
            emotion = NPCEmotion;
            emotionPL = PlayerEmotion;
        }
    }
}
