using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter
{
    public class VideoPlayer
    {
        public MediaPlayer MediaPlayer { get; }

        public VideoPlayer(LibVLC libVLC)
        {
            MediaPlayer = new MediaPlayer(libVLC);
        }

        public void Play(Media media)
        {
            MediaPlayer.Play(media);
        }

        public void Stop() => MediaPlayer.Stop();

        internal void SetSpeed(int speed)
        {
            MediaPlayer.SetRate(speed);
        }

        internal void SetPause(bool value)
        {
            MediaPlayer.SetPause(value);
        }
    }
}
