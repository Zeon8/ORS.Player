namespace ORS.Parser.Commands
{
    public class SelectCommand : BaseCommand
    {
        public string OptionA { get; private set; } = null!;
        public string OptionB { get; private set; } = null!;

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void Parse(StringReader reader)
        {
            OptionA = reader.ReadTo('\t', false).TrimStart();
            OptionB = reader.ReadTo('\t', false).TrimStart();
        }
    }
}
