namespace ORS.Parser.Commands
{
    public class PlayMovieCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string Layer { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadTo('\t');
            Layer = reader.ReadTo('\t');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
