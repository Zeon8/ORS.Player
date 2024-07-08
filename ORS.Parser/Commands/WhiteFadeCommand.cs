namespace ORS.Parser.Commands
{
    public class WhiteFadeCommand : FadeCommand
    {
        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
