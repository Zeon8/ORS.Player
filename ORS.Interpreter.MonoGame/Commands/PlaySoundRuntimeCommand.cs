using Microsoft.Xna.Framework.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame.Commands
{
    public class PlaySoundRuntimeCommand : RuntimeCommand
    {
        private readonly int _channel;
        private readonly WaveStream _sound;
        private readonly WaveOutEvent _waveOutEvent = new();

        public PlaySoundRuntimeCommand(TimeSpan beginTime, TimeSpan endTime,
            int channel, WaveStream sound)
            : base(beginTime, endTime)
        {
            _channel = channel;
            _sound = sound;

            _waveOutEvent.Init(_sound);
            _waveOutEvent.Play();
            _waveOutEvent.Pause();
        }

        ~PlaySoundRuntimeCommand()
        {
            _sound.Dispose();
            _waveOutEvent.Dispose();
            
        }

        public override void Begin()
        {
            _waveOutEvent.Play();
        }

        public override void End()
        {
            _waveOutEvent.Stop();
        }
    }
}
