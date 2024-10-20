using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ORS.Player
{
    public class FadeScreen
    {
        private bool _isPlaying;
        private float _elapsedTime;
        private float _duration;
        private float _alpha;

        private FadeEffect _fadeEffect;
        private Color _color;

        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _device;

        public FadeScreen(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            _spriteBatch = spriteBatch;
            _device = device;

            _texture = new Texture2D(_device, 1, 1);
            _texture.SetData(new Color[] { Color.White });
        }

        public void Fade(Color color, TimeSpan duration, FadeEffect fadeEffect)
        {
            _isPlaying = true;
            _color = color;
            _duration = (float)duration.TotalSeconds;
            _fadeEffect = fadeEffect;
        }

        public void Draw()
        {
            if (_isPlaying)
                _spriteBatch.Draw(_texture, _device.Viewport.Bounds, _color * _alpha);
        }

        public void Update(float deltaTime)
        {
            if (!_isPlaying)
                return;

            if (_elapsedTime < _duration)
                _elapsedTime += deltaTime;

            _alpha = Math.Clamp(_elapsedTime / _duration, 0, 1);

            if (_fadeEffect == FadeEffect.In)
                _alpha = 1 - _alpha; // Reversed
        }

        public void Stop()
        {
            _isPlaying = false;
            _elapsedTime = 0;
        }
    }


    public enum FadeEffect
    {
        In,
        Out
    }
}
