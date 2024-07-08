using MonoGame.Extended.Framework.Media;
using System;
using System.Diagnostics;
using System.Threading;

namespace ORS.Interpreter.MonoGame.Commands
{
    public class PlayMovieRuntimeCommand : RuntimeCommand
    {
        private readonly Video _video;
        private readonly VideoPlayer _player;

        //For debug purposes
        private readonly string _file;

        public PlayMovieRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            VideoPlayer player, Video video, string file) 
            : base(beginTime, endTime)
        {
            _video = video;
            _file = file;
            _player = player;

        }

        //~PlayMovieRuntimeCommand()
        //{
        //    _video.Dispose();
        //}

        public override void Begin()
        {
            _player.Play(_video);
        }

        public override void End()
        {
            _player.Stop();
        }
    }
}
