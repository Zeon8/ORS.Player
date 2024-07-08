using Microsoft.Xna.Framework.Graphics;
using System;

namespace ORS.Interpreter.MonoGame.Commands
{
    public abstract class RuntimeCommand : IRuntimeCommand
    {
        protected RuntimeCommand(TimeSpan beginTime, TimeSpan endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public TimeSpan BeginTime { get; }

        public TimeSpan EndTime { get; }

        public bool IsRunning { get; set; }

        public virtual void Begin(){}

        public virtual void Update() {}

        public virtual void Draw(SpriteBatch spriteBatch){}

        public virtual void End(){}
    }
}
