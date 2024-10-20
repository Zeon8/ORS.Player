using Microsoft.Xna.Framework.Graphics;
using System;

namespace ORS.Interpreter.MonoGame
{
    public interface IRuntimeCommand
    {
        TimeSpan StartTime { get; }
        TimeSpan EndTime { get; }

        bool IsRunning { get; set; }

        void Start();
        void Stop();
        void Update();
    }
}
