using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame
{
    public class Subtitles
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        private string _subtitles;
        private Vector2 _size;
        private Vector2 _position;
        private const int TextBottomOffset = 30;
        private static readonly Color s_textColor = Color.White;
        private readonly Rectangle _screenRect;
        private string _currentText;

        public Subtitles(SpriteBatch spriteBatch, SpriteFont font, Rectangle screenRect)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _screenRect = screenRect;
        }
         
        public void Draw()
        {
            if (string.IsNullOrEmpty(_subtitles))
                return;

            _spriteBatch.DrawString(_font, _subtitles, _position, s_textColor);
        }

        public void ShowSubtitles(string subtitles)
        {
            _subtitles = subtitles.Replace("\\n","\n");

            _size = _font.MeasureString(_subtitles);
            if(_size.X > _screenRect.Width)
            {
                int length = _subtitles.Length / 2;
                _subtitles = _subtitles[..length] + '\n' + _subtitles[length..];
                _size = _font.MeasureString(_subtitles);
            }

            float x = _screenRect.Width / 2 - _size.X / 2;
            float y = _screenRect.Height - TextBottomOffset - _size.Y / 2;
            _position = new Vector2(x, y);
            _currentText = subtitles;
        }

        public void Hide(string subtitles)
        {
            if(subtitles == _currentText)
                _subtitles = null;
        }
    }
}
