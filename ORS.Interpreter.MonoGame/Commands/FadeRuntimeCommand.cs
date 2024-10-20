using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Player.Commands
{
    public class FadeRuntimeCommand : RuntimeCommand
    {
        private FadeEffect _fadeEffect;
        private FadeScreen _fadeScreen;
        private Color _color;

        public FadeRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            FadeEffect fadeEffect, Color color, FadeScreen fadeScreen)
            : base(beginTime, endTime)
        {
            _fadeEffect = fadeEffect;
            _fadeScreen = fadeScreen;
            _color = color;
        }

        public override void Start()
        {
            var duration = EndTime - StartTime;
            _fadeScreen.Fade(_color, duration, _fadeEffect);
        }

        public override void Stop()
        {
            _fadeScreen.Stop();
        }
    }
}
