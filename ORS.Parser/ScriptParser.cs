using ORS.Parser.Commands;

namespace ORS.Parser
{
    public class ScriptParser
    {
        private StringReader? _reader;
        private bool _legacy;

        public ScriptParser() { }

        public ScriptParser(StringReader reader, bool legacy = false)
        {
            Load(reader);
            _legacy = legacy;
        }

        public void Load(StringReader reader, bool legacy = false)
        {
            _reader = reader;
            _legacy = legacy;
        }

        public ICommand? ParseNextCommand()
        {
            _reader.SkipToSymbol('[');
            string type = _reader.ReadToSymbol(']');
            _reader.SkipToSymbol('=');
            if(_legacy)
                _reader.SkipToSymbol('"');

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
                "Next" => new NextCommand(),
                "SetSELECT" => new SelectCommand(),
                _ => throw new Exception($"Unknown command: {type}")
            };

            if (_legacy)
                command?.ParseLegacyFormat(_reader);
            else
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
