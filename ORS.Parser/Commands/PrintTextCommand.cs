namespace ORS.Parser.Commands
{
    public class PrintTextCommand : BaseCommand
    {
        public string Text { get; private set; } = null!;
        public string Character { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Character = reader.ReadToSymbol('\t');
            Text = reader.ReadToSymbol('\t', ignoreSpaces: false);
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            Character = reader.ReadToSymbol(',');
            Text = reader.ReadToSymbol(',', ignoreSpaces: false);
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
