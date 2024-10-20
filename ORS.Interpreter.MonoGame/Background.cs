using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ORS.Player
{
    public class Background
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Rectangle _drawRect;
        private Texture2D _texture;

        public Background(SpriteBatch spriteBatch, Rectangle drawRect)
        {
            _spriteBatch = spriteBatch;
            _drawRect = drawRect;
        }

        public void Draw()
        {
            if(_texture is not null)
                _spriteBatch.Draw(_texture, _drawRect, Color.White);
        }

        public void Show(Texture2D texture) => _texture = texture;

        public void Hide(Texture2D texture)
        {
            if(texture == _texture)
                _texture = null;
        }
    }
}
