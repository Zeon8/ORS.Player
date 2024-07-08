using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace ORS.Interpreter.MonoGame
{
    public class AudioPlayer
    {
        private Dictionary<int, WaveOutEvent> _players = new();

        private readonly WaveOutEvent _voicePlayer = new();
        private readonly WaveOutEvent _backgroundPlayer = new();
        private WaveStream _loopAudio;

        public AudioPlayer()
        {
            //_backgroundPlayer.PlaybackStopped += BackgrkoundPlayer_PlaybackStopped;
        }

        public void PlaySound(int layer, IWaveProvider sound)
        {
            if (!_players.TryGetValue(layer, out WaveOutEvent player))
            {
                player = new WaveOutEvent();
                _players.Add(layer, player);
            }
            player.Init(sound);
            player.Play();
        }

        public void StopSound(int layer)
        {
            if (_players.TryGetValue(layer, out WaveOutEvent player))
                player.Stop();
        }

        public void PlayVoice(IWaveProvider voice)
        {
            if (_voicePlayer.PlaybackState == PlaybackState.Playing)
                return;

            _voicePlayer.Init(voice);
            _voicePlayer.Play();
        }

        public void StopVoice() => _voicePlayer.Stop(); 

        public void PlayBackground(WaveStream intro, WaveStream loop)
        {
            _loopAudio = loop;
            _backgroundPlayer.Init(intro);
            _backgroundPlayer.Play();
        }

        

        public void StopBackground()
        {
            _backgroundPlayer.Stop();
        }
    }
}
