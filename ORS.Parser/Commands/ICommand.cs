namespace ORS.Parser.Commands
{
    public interface ICommand
    {
        internal void Parse(StringReader reader);
        internal void ParseLegacyFormat(StringReader reader);

        void Accept(ICommandVisitor visitor);
    }
}
