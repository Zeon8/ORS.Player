using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ORS.Player.Components
{
    public class LipSyncAnimator
    {
        private readonly TimeSpan AnimationInterval = TimeSpan.FromMilliseconds(10);

        private IReadOnlyList<Texture2D> _frames;
        private int _currentFrame;
        private bool _playing;
        private TimeSpan _time;

        private readonly Rectangle _rect;
        private readonly SpriteBatch _spriteBatch;

        public LipSyncAnimator(Rectangle rect, SpriteBatch spriteBatch)
        {
            _rect = rect;
            _spriteBatch = spriteBatch;
        }

        public void Play(IReadOnlyList<Texture2D> frames)
        {
            _frames = frames;
            _currentFrame = 0;
            _time = TimeSpan.Zero;
            _playing = true;
        }

        public void Stop() => _playing = false;

        public void Draw()
        {
            if (_playing)
            {
                var frame = _frames[_currentFrame];
                _spriteBatch.Draw(frame, _rect, Color.White);
            }
        }

        public void Update(TimeSpan deltaTime)
        {
            if (_playing)
            {
                if (_time < AnimationInterval)
                {
                    _time += deltaTime;
                    return;
                }
                _time = TimeSpan.Zero;

                if (_currentFrame == 2)
                    _currentFrame = Random.Shared.Next(1, 3);
                else
                    _currentFrame = Random.Shared.Next(0, 3);
            }
        }
    }
}
