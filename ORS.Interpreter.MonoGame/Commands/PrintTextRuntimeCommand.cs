using System;

namespace ORS.Interpreter.MonoGame.Commands
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

        public override void Begin()
        {
            _subtitles.ShowSubtitles(_text);
        }

        public override void End()
        {
            _subtitles.Hide();
        }
    }
}
