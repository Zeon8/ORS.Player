namespace ORS.Parser.Commands
{
    public class SkipFrameCommand : ICommand
    {
        public string Time { get; private set; } = null!;

        void ICommand.Parse(StringReader reader)
        {
            Time = reader.ReadTo(';');
        }

        public void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}