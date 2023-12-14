
namespace ORS.Parser.Commands
{
    public class BlackFadeCommand : BaseCommand
    {
        public string Type { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Type = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            Type = reader.ReadToSymbol(',');
        }

        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
