using NAudio.Wave;
using System;

namespace ORS.Interpreter.MonoGame.Commands
{
    public class PlayVoiceRuntimeCommand : RuntimeCommand
    {
        private readonly IWaveProvider _voice;
        private readonly WaveOutEvent _waveOutEvent = new();

        public PlayVoiceRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            IWaveProvider voice, AudioPlayer player) 
            : base(beginTime, endTime)
        {
            _voice = voice;
            _waveOutEvent.Init(_voice);
            _waveOutEvent.Play();
            _waveOutEvent.Pause();
        }

        public override void Begin()
        {
            //_player.PlayVoice(_voice);
            _waveOutEvent.Play();
        }

        public override void End()
        {
        }
    }
}
