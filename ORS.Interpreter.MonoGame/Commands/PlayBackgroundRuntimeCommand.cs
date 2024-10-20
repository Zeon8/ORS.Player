using Microsoft.Xna.Framework.Audio;
using System;

namespace ORS.Player.Commands
{
    public class PlayBackgroundRuntimeCommand : RuntimeCommand
    {
        private readonly SoundEffectInstance _intro;
        private readonly SoundEffectInstance _loop;
        private readonly SoundManager _soundManager;

        public PlayBackgroundRuntimeCommand(TimeSpan beginTime, TimeSpan endTime,
            SoundEffectInstance intro, SoundEffectInstance loop, SoundManager soundManager) 
            : base(beginTime, endTime)
        {
            _intro = intro;
            _loop = loop;
            _soundManager = soundManager;
            _loop.IsLooped = true;
        }

        public override void Start()
        {
            _soundManager.Play(_intro);
        }

        public override void Update()
        {
            if (_intro.State == SoundState.Stopped && _loop.State == SoundState.Stopped)
                _loop.Play();
        }

        public override void Stop()
        {
            if (_intro.State == SoundState.Playing)
                _soundManager.Stop(_intro);
            if (_intro.State == SoundState.Playing)
                _soundManager.Stop(_loop);
        }
    }
}
