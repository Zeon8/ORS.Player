namespace ORS.Parser.Commands
{
    public class NextCommand : ICommand
    {
        public string Time { get; private set; } = null!;

        void ICommand.Parse(StringReader reader)
        {
            Time = reader.ReadTo(';');
        }

        void ICommand.Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
