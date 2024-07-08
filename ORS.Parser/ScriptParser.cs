using ORS.Parser.Commands;

namespace ORS.Parser
{
    public class ScriptParser
    {
        private readonly StringReader? _reader;

        public ScriptParser(StringReader reader)
        {
            _reader = reader;
        }

        public static ScriptParser LoadFromString(string script)
        {
            var reader = new StringReader(script);
            return new ScriptParser(reader);
        }

        public ICommand? ParseNextCommand()
        {
            _reader.SkipTo('[');
            string type = _reader.ReadTo(']');
            _reader.SkipTo('=');

            if (_reader.Peek() == -1)
                return null;

            ICommand? command = type switch
            {
                "SkipFRAME" => new SkipFrameCommand(),
                "PlaySe" => new PlaySoundCommand(),
                "PlayMovie" => new PlayMovieCommand(),
                "PlayBgm" => new PlayBackgroundCommand(),
                "CreateBG" => new CreateBackgroundCommand(),
                "PrintText" => new PrintTextCommand(),
                "PlayVoice" => new PlayVoiceCommand(),
                "BlackFade" => new BlackFadeCommand(),
                "WhiteFade" => new WhiteFadeCommand(),
                "Next" => new NextCommand(),
                "SetSELECT" => new SelectCommand(),
                _ => throw new Exception($"Unknown command: {type}")
            };

            command?.Parse(_reader);

            return command;
        }

        public IEnumerable<ICommand> ParseCommands()
        {
            while (ParseNextCommand() is ICommand command)
                yield return command;
        }

        public void VisitCommands(ICommandVisitor visitor)
        {
            foreach (var command in ParseCommands())
            {
                command.Accept(visitor);
            }
        }
    }
}
