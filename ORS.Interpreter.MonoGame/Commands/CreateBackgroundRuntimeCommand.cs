using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ORS.Player.Commands
{
    public class CreateBackgroundRuntimeCommand : RuntimeCommand
    {
        private readonly Texture2D _image;
        private readonly Background _background;

        public CreateBackgroundRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, 
            Texture2D image, Background background) 
            : base(beginTime, endTime)
        {
            _image = image;
            _background = background;
        }

        public override void Start()
        {
            _background.Show(_image);
        }

        public override void Stop()
        {
            _background.Hide(_image);
        }
    }
}
