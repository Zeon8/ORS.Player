using Microsoft.Xna.Framework.Graphics;
using ORS.Interpreter.MonoGame;
using System;

namespace ORS.Player.Commands
{
    public abstract class RuntimeCommand : IRuntimeCommand
    {
        protected RuntimeCommand(TimeSpan beginTime, TimeSpan endTime)
        {
            StartTime = beginTime;
            EndTime = endTime;
        }

        public TimeSpan StartTime { get; }

        public TimeSpan EndTime { get; }

        public bool IsRunning { get; set; }

        public virtual void Start() { }

        public virtual void Stop() { }

        public virtual void Update() { }
    }
}
