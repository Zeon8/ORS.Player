namespace ORS.Parser.Commands
{
    public class PrintTextCommand : BaseCommand
    {
        public string Text { get; private set; } = null!;
        public string Character { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Character = reader.ReadTo('\t');
            Text = reader.ReadTo('\t', ignoreSpaces: false);
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
