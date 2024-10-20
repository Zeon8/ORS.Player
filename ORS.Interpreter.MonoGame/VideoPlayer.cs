using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ORS.Player
{
    public class VideoPlayer : IDisposable
    {
        private VideoDecoder _decoder;
        private Texture2D _currentFrame;

        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _device;
        private readonly Rectangle _screenRect;

        public VideoPlayer(GraphicsDevice device, SpriteBatch spriteBatch, Rectangle screenRect)
        {
            _device = device;
            _spriteBatch = spriteBatch;
            _screenRect = screenRect;
        }

        public void Play(VideoDecoder decoder)
        {
            _decoder = decoder;
        }

        public void Stop()
        {
            _decoder = null;
            _currentFrame = null;
        }

        public void Update()
        {
            _currentFrame = _decoder?.GetNextFrame(_device);
        }

        public void Draw()
        {
            if(_currentFrame is not null)
                _spriteBatch.Draw(_currentFrame, _screenRect, Color.White);
        }

        public void Dispose() => _decoder.Dispose();
    }
}
