using System;
using System.Diagnostics;

namespace ORS.Interpreter
{
    internal class SimpleLogger
    {
        private TimeSpan _time;

        public SimpleLogger()
        {
            _time = DateTime.Now.TimeOfDay;
        }

        public void Log(string time, string messsage)
        {
            var realTime = DateTime.Now.TimeOfDay - _time;
            Debug.WriteLine($"[{realTime}][{time}]: {messsage}");
        }
    }
}