using Microsoft.Xna.Framework.Graphics;
using ORS.Player.Components;
using ORS.Player.Media;
using System;
using System.Collections.Generic;

namespace ORS.Player.Commands
{
    public class PlayMovieRuntimeCommand : RuntimeCommand
    {
        private readonly VideoPlayer _player;
        private readonly Video _video;

        public PlayMovieRuntimeCommand(TimeSpan beginTime, TimeSpan endTime,
            VideoPlayer player, Video video)
            : base(beginTime, endTime)
        {
            _player = player;
            _video = video;
        }

        public override void Start() => _player.Play(_video);

        public override void Stop() => _player.Stop();
    }
}
