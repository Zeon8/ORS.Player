﻿
namespace ORS.Parser.Commands
{
    public class BlackFadeCommand : FadeCommand
    {
        public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    }
}
