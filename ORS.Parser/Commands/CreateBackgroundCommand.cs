
namespace ORS.Parser.Commands
{
    public class CreateBackgroundCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;
        public string BackgroundType { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            BackgroundType = reader.ReadToSymbol('\t');
            Path = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            BackgroundType = reader.ReadToSymbol(',');
            Path = reader.ReadToSymbol(',');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
