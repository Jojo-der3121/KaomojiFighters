using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.BitmapFonts;

namespace KaomojiFighters
{
    class SpeechBubble
    {
        private readonly Vector2 _size;
        private readonly Vector2 _location;
        private readonly List<string> _speech = new List<string>();
        private readonly Color _boxColor;
        private readonly Color _textColor;
        private readonly float _fontSize = 5f;
        private readonly bool _isHudComponent;

        public SpeechBubble(Vector2 location, string text, Vector2 size, bool isHUDComponent, float scale)
        {
            _size = size;
            _location = location;
            _isHudComponent = isHUDComponent;
            _fontSize = scale;
            if (_isHudComponent)
            {
                _boxColor = new Color(104, 201, 52);
                _textColor = Color.Black;
            }
            else
            {
                _boxColor = new Color(Color.Black, 175);
                _textColor = Color.CornflowerBlue;
            }
            GetSpeech(text);
        }

        public SpeechBubble(Vector2 location, string text, float fontSize, Vector2 size, Color outerColor, Color textColor, bool isHUDComponent)
        {
            _boxColor = outerColor;
            _textColor = textColor;
            _size = size;
            _location = location;
            _fontSize = fontSize;
            _isHudComponent = isHUDComponent;
            GetSpeech(text);
        }

        private void GetSpeech(string text)
        {
            
            if (Graphics.Instance.BitmapFont.MeasureString(text).X*_fontSize <= _size.X -60) _speech.Add(text);
            else
            {
                _speech.Add(text.Substring(0, text.Length / (int)Math.Round(Graphics.Instance.BitmapFont.MeasureString(text).X * _fontSize / (_size.X - 60))));
                text = text.Remove(0, text.Length / (int)Math.Round(Graphics.Instance.BitmapFont.MeasureString(text).X * _fontSize / (_size.X - 60)));
                while (Graphics.Instance.BitmapFont.MeasureString(text).X *_fontSize > _size.X -60)
                {
                    _speech.Add( text.Substring(0, text.Length / (int)Math.Round(Graphics.Instance.BitmapFont.MeasureString(text).X * _fontSize / (_size.X - 60))));
                    text = text.Remove(0,
                        text.Length / (int)Math.Round(Graphics.Instance.BitmapFont.MeasureString(text).X * _fontSize / (_size.X - 60)));
                }
                if(text != "") _speech.Add( text);
            }
        }

        // if is HUD Component => it'll be 20 pixels smaller -> good for selectors.

        public void DrawTextField(Batcher batcher)
        {
            var yLocation = (_size.Y - 60 + Graphics.Instance.BitmapFont.MeasureString("Ag").Y) / 2 + Graphics.Instance.BitmapFont.MeasureString("Ag").Y *
                (_speech.Count - 1)* _fontSize;
            if (!_isHudComponent) batcher.DrawRect(_location.X - _size.X / 2, _location.Y - _size.Y / 2, _size.X, yLocation+60  < _size.Y? _size.Y : yLocation+60 , _boxColor);
            batcher.DrawRect(_location.X -( _size.X-20) / 2, _location.Y - (_size.Y - 20) / 2, (_size.X - 20),  yLocation + 60 < _size.Y ? (_size.Y - 20) : yLocation + 40, _textColor); 
            batcher.DrawRect(_location.X -( _size.X-40) / 2, _location.Y - (_size.Y - 40) / 2, (_size.X - 40), yLocation + 60 < _size.Y ? (_size.Y - 40) : yLocation + 20, _boxColor); 
            for (var str = 0; str < _speech.Count; str++)
            {
                batcher.DrawString(Graphics.Instance.BitmapFont, _speech[str], new Vector2(_location.X - (_size.X - 60)/2,
                    _location.Y - (_size.Y - 60 + Graphics.Instance.BitmapFont.MeasureString(_speech[str]).Y) / 2 + Graphics.Instance.BitmapFont.MeasureString(_speech[str]).Y * str * _fontSize), _textColor, 0f, Vector2.Zero, _fontSize, SpriteEffects.None, 0f);
            }
        }
    }
}
