namespace ORS.Parser.Commands
{
    public class PlaySoundCommand : BaseCommand
    {
        public string Layer { get; private set; } = null!;
        public string Path { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Layer = reader.ReadTo('\t');
            Path = reader.ReadTo('\t');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
