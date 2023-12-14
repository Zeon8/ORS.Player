namespace ORS.Parser.Commands
{
    public class PlayMovieCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string Layer { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadToSymbol('\t');
            Layer = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {    
            Path = reader.ReadToSymbol(',');
            Layer = reader.ReadToSymbol(',');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
