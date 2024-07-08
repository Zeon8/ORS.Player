using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;

namespace ORS.Interpreter
{
    public class AudioPlayer
    {
        private readonly MediaPlayer _backgroundPlayer;
        private readonly MediaPlayer _voicePlayer;

        private readonly Dictionary<int, MediaPlayer> _soundPlayers = new();
        private readonly Dictionary<MediaPlayer, TimeSpan> _currentPlaying = new();
        private readonly LibVLC _libVlc;

        public AudioPlayer(LibVLC libVlc)
        {
            _libVlc = libVlc;
            _backgroundPlayer = new MediaPlayer(_libVlc);
            _voicePlayer = new MediaPlayer(_libVlc);
        }

        private MediaPlayer GetPlayer(Dictionary<int, MediaPlayer> players, int channel)
        {
            if (!players.TryGetValue(channel, out MediaPlayer? player))
            {
                player = new(_libVlc);
                players.Add(channel, player);
            }
            return player;
        }

        public MediaPlayer GetSoundPlayer(int channel) => GetPlayer(_soundPlayers, channel);

        public void PlayVoice(Media media, TimeSpan start, bool autoPlay) => Play(_voicePlayer, media, start, autoPlay);
        public void StopVoice() => StopPlayer(_voicePlayer);

        public void Play(MediaPlayer player, Media media, TimeSpan start, bool autoPlay)
        {
            player.Media = media;
            if (autoPlay)
                player.Play();

            _currentPlaying.TryAdd(player, start);
        }

        public void StopPlayer(MediaPlayer player)
        {
            player.Stop();
            _currentPlaying.Remove(player);
        }

        public void Stop()
        {
            foreach (var item in _currentPlaying)
                item.Key?.Stop();
            _currentPlaying.Clear();
        }

        public void SetSpeed(TimeSpan currentTime, int speed)
        {
            foreach (var item in _currentPlaying)
            {
                if (speed > 1)
                {
                    item.Key.SetPause(true);
                    continue;
                }
                TimeSpan start = item.Value;
                MediaPlayer player = item.Key;

                TimeSpan position = currentTime - start;
                player.Position = (float)(position.TotalMilliseconds / player.Length);
                player.Play();
            }
        }

        public void PlayBackground(Media intro, TimeSpan start, bool autoPlay)
        {
            Play(_backgroundPlayer, intro, start, autoPlay);
        }

        public void StopBackground() => StopPlayer(_backgroundPlayer);


        internal void SetPause(bool pause)
        {
            foreach (var item in _currentPlaying)
            {
                item.Key.SetPause(pause);
            }
        }
        public void Dispose()
        {
            foreach (var item in _soundPlayers)
                item.Value.Dispose();
            _backgroundPlayer.Dispose();
            _voicePlayer.Dispose();
        }

        
    }
}
