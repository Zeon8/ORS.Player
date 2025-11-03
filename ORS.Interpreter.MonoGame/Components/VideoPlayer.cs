using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ORS.Player.Media;
using System;

namespace ORS.Player.Components
{
    public class VideoPlayer : IDisposable
    {
        private Video _video;
        private int _frameIndex;
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

        public void Play(Video video)
        {
            _frameIndex = 0;
            _video = video;
        }

        public void Stop()
        {
            _video = null;
            _currentFrame = null;
            _frameIndex = 0;
        }

        public void Update()
        {
            if (_video is null || _frameIndex >= _video.Frames.Count)
                return;

            _currentFrame = _video.Frames[_frameIndex];
            _frameIndex++;
        }

        public void Draw()
        {
            if (_currentFrame is not null)
                _spriteBatch.Draw(_currentFrame, _screenRect, Color.White);
        }

        public void Dispose() => _video.Dispose();
    }
}
