namespace ORS.Parser.Commands
{
    public class PlayVoiceCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string Character { get; private set; } = null!;
        public string IsMale { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadTo('\t');
            IsMale = reader.ReadTo('\t');
            Character = reader.ReadTo('\t');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
