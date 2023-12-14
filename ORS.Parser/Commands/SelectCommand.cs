using ORS.Parser;
using ORS.Parser.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Parser.Commands
{
    public class SelectCommand : BaseCommand
    {
        public string OptionA { get; private set; } = null!;
        public string OptionB { get; private set; } = null!;

        public override void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void Parse(StringReader reader)
        {
            OptionA = reader.ReadToSymbol('\t', false).TrimStart();
            OptionB = reader.ReadToSymbol('\t', false).TrimStart();
        }

        protected override void ParseLegacyFormat(StringReader reader)
        {
            OptionA = reader.ReadToSymbol(',', false).TrimStart();
            OptionB = reader.ReadToSymbol(',', false).TrimStart();
        }
    }
}
