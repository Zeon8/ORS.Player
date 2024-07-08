using System;
using System.Diagnostics;

namespace ORS.Interpreter
{
    internal class SimpleLogger
    {
        private long _time;

        public SimpleLogger()
        {
            Setup();
        }

        public void Log(TimeSpan time, string messsage)
        {
            var realTime = Stopwatch.GetElapsedTime(_time);
            Debug.WriteLine($"[p:{time}, r:{realTime}]: {messsage}");
        }

        public void Setup()
        {
            _time = Stopwatch.GetTimestamp();
        }
    }
}