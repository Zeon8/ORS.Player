using Microsoft.Xna.Framework.Audio;
using System;

namespace ORS.Player.Commands
{
    public class PlaySoundRuntimeCommand : RuntimeCommand
    {
        private readonly SoundEffectInstance _sound;
        private readonly SoundManager _soundManager;

        public PlaySoundRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            SoundEffectInstance sound, SoundManager soundManager) : base(beginTime, endTime)
        {
            _sound = sound;
            _soundManager = soundManager;
        }

        public override void Start()
        {
            _soundManager.Play(_sound);
        }

        public override void Stop()
        {
            _soundManager.Stop(_sound);
        }
    }
}
