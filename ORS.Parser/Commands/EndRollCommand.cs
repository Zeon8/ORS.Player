using ORS.Parser.Commands;

namespace ORS.Parser
{
    public class EndRollCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadTo('\t');
        }
    }
}