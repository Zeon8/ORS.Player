
namespace ORS.Parser.Commands
{
    public class CreateBackgroundCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string BackgroundType { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            BackgroundType = reader.ReadTo('\t');
            Path = reader.ReadTo('\t');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
