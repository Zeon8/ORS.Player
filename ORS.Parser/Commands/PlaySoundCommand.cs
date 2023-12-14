namespace ORS.Parser.Commands
{
    public class PlaySoundCommand : BaseCommand
    {
        public string Layer { get; private set; } = null!;
        public string Path { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Layer = reader.ReadToSymbol('\t');
            Path = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            Layer = reader.ReadToSymbol(',');
            Path = reader.ReadToSymbol(',');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
