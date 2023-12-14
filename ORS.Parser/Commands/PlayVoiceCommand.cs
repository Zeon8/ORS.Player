namespace ORS.Parser.Commands
{
    public class PlayVoiceCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string Character { get; private set; } = null!;
        public string IsMale { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadToSymbol('\t');
            IsMale = reader.ReadToSymbol('\t');
            Character = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            Path = reader.ReadToSymbol(',');
            IsMale = reader.ReadToSymbol(',');
            Character = reader.ReadToSymbol(',');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
