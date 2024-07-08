using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame
{
    public class Subtitles
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly GameWindow _window;

        private string _subtitles;
        private Vector2 _size;
        private Vector2 _position;
        private const int TextBottomOffset = 30;
        private static readonly Color s_textColor = Color.White;

        public Subtitles(SpriteBatch spriteBatch, SpriteFont font, GameWindow window)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _window = window;
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
            var x = _window.ClientBounds.Width / 2 - _size.X / 2;
            var y = _window.ClientBounds.Height - TextBottomOffset - _size.Y/2;
            _position = new Vector2(x, y);
        }

        public void Hide() => _subtitles = null;
    }
}
