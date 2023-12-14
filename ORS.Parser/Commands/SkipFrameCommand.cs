namespace ORS.Parser.Commands
{
    public class SkipFrameCommand : ICommand
    {
        public string Time { get; private set; } = null!;

        void ICommand.Parse(StringReader reader)
        {
            Time = reader.ReadToSymbol(';');
        }

        void ICommand.ParseLegacyFormat(StringReader reader)
        {
            Time = reader.ReadToSymbol('"');
        }

        public void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}