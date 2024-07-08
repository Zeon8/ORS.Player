using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Interpreter.MonoGame
{
    public interface IRuntimeCommand
    {
        TimeSpan BeginTime { get; }

        TimeSpan EndTime { get; }

        bool IsRunning { get; set; }

        void Begin();

        void Draw(SpriteBatch spriteBatch);

        void End();
    }
}
