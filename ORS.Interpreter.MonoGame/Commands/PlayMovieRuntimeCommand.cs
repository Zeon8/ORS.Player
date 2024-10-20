using Microsoft.Xna.Framework.Graphics;
using System;

namespace ORS.Player.Commands
{
    public class PlayMovieRuntimeCommand : RuntimeCommand
    {
        private readonly VideoDecoder _video;
        private readonly VideoPlayer _player;

        public PlayMovieRuntimeCommand(TimeSpan beginTime, TimeSpan endTime,
            VideoPlayer player, VideoDecoder video)
            : base(beginTime, endTime)
        {
            _video = video;
            _player = player;
        }

        public override void Start() => _player.Play(_video);

        public override void Stop() => _player.Stop();
    }
}
