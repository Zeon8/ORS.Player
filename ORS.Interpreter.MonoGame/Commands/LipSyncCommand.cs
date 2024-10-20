using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ORS.Player.Commands
{
    public class LipSyncCommand : RuntimeCommand
    {
        private readonly IReadOnlyList<Texture2D> _frames;
        private readonly LipSyncAnimator _lipSyncAnimator;

        public LipSyncCommand(TimeSpan beginTime, TimeSpan endTime, 
            IReadOnlyList<Texture2D> frames, LipSyncAnimator lipSync)
            : base(beginTime, endTime)
        {
            _frames = frames;
            _lipSyncAnimator = lipSync;
        }

        public override void Start()
        {
            _lipSyncAnimator.Play(_frames);
        }

        public override void Stop()
        {
            _lipSyncAnimator.Stop();
        }
    }
}
