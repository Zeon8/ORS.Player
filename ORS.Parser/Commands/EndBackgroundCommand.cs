namespace ORS.Parser.Commands
{
    public class EndBackgroundCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadTo('\t');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}