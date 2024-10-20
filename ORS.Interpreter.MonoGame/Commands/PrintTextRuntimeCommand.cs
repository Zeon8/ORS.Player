using ORS.Interpreter.MonoGame;
using System;
using System.Diagnostics;

namespace ORS.Player.Commands
{
    public class PrintTextRuntimeCommand : RuntimeCommand
    {
        private readonly string _text;
        private readonly Subtitles _subtitles;

        public PrintTextRuntimeCommand(TimeSpan beginTime, TimeSpan endTime, string text, Subtitles subtitles)
            : base(beginTime, endTime)
        {
            _text = text;
            _subtitles = subtitles;
        }

        public override void Start()
        {
            _subtitles.ShowSubtitles(_text);
        }

        public override void Stop()
        {
            _subtitles.Hide(_text);
        }
    }
}
