using ORS.Parser.Commands;
using System;

namespace ORS.Player
{
    public static class CommandExtensions
    {
        private static TimeSpan ParseTime(string value)
        {
            string[] values = value.Split(':');
            int minutes = int.Parse(values[0]);
            int seconds = int.Parse(values[1]);
            int miliseconds = int.Parse(values[2]) * 10;

            var time = new TimeSpan(0, 0, minutes, seconds, miliseconds);
            return time;
        }

        public static void ParseTime(this BaseCommand command, out TimeSpan start, out TimeSpan end)
        {
            start = ParseTime(command.StartTime);
            end = ParseTime(command.EndTime);
        }
    }
}
