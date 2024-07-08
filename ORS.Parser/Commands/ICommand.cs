namespace ORS.Parser.Commands
{
    public interface ICommand
    {
        internal void Parse(StringReader reader);

        void Accept(ICommandVisitor visitor);
    }
}
