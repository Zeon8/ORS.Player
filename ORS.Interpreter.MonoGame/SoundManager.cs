using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace ORS.Player
{
    public class SoundManager
    {
        private HashSet<SoundEffectInstance> _sounds = new();

        public void Play(SoundEffectInstance sound)
        {
            sound.Play();
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
    }
}
