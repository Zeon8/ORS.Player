using ORS.Parser;
using ORS.Parser.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Parser.Commands
{
    public class PlayBackgroundCommand : BaseCommand
    {
        public string Path { get; private set; } = null!;

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void Parse(StringReader reader)
        {
            Path = reader.ReadToSymbol('\t');
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            Path = reader.ReadToSymbol(',');
        }
    }
}
