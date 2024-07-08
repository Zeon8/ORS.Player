using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame.Commands
{
    public class PlayBackgroundRuntimeCommand : RuntimeCommand
    {
        private readonly WaveStream _intro;
        private readonly WaveStream _loop;

        private readonly WaveOutEvent _waveOutEvent = new();
        private readonly WaveOutEvent _waveOutEventLoop = new();

        public PlayBackgroundRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            WaveStream intro, WaveStream loop) 
            : base(beginTime, endTime)
        {
            _intro = intro;
            _loop = loop;

            _waveOutEvent.Init(_intro);
            _waveOutEvent.PlaybackStopped += OnPlaybackStopped;
            _waveOutEvent.Play();
            _waveOutEvent.Pause();

            _waveOutEventLoop.Init(_loop);
            _waveOutEventLoop.PlaybackStopped += OnPlaybackStopped;
            _waveOutEventLoop.Play();
            _waveOutEventLoop.Pause();
        }

        ~PlayBackgroundRuntimeCommand()
        {
            _intro.Dispose();
            _loop.Dispose();
            _waveOutEventLoop.Dispose();
        }

        public override void Begin()
        {
            _waveOutEvent.Play();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            _waveOutEventLoop.Play();
        }

        public override void End()
        {
            _waveOutEvent.Stop();
            _waveOutEventLoop.Stop();
        }

    }
}
