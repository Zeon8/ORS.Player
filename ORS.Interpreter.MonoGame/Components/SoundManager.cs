using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace ORS.Player.Components
{
    public class SoundManager
    {
        private readonly List<SoundEffectInstance> _sounds = new();
        private float _pitch = 0f;

        public void Play(SoundEffectInstance sound)
        {
            sound.Play();
            sound.Pitch = _pitch;
            _sounds.Add(sound);
        }

        public void Stop(SoundEffectInstance sound)
        {
            sound.Stop();
            _sounds.Remove(sound);
        }

        public void SetPaused(bool value)
        {
            foreach (SoundEffectInstance sound in _sounds)
            {
                if (value)
                    sound.Pause();
                else
                    sound.Play();
            }
        }

        public void SetSpeed(float value)
        {
            _pitch = 1 - (1 / value);
            foreach (var sound in _sounds)
                sound.Pitch = _pitch;
        }
    }
}
