using ORS.Parser;
using ORS.Parser.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Parser.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public string StartTime { get; private set; } = null!;
        public string EndTime { get; private set; } = null!;

        public abstract void Accept(ICommandVisitor visitor);
        protected abstract void Parse(StringReader reader);
        protected abstract void ParseLegacyFormat(StringReader reader);

        void ICommand.Parse(StringReader reader)
        {
            StartTime = reader.ReadToSymbol('\t');
            Parse(reader);
            EndTime = reader.ReadToSymbol(';');
        }

        void ICommand.ParseLegacyFormat(StringReader reader)
        {
            StartTime = reader.ReadToSymbol(',');
            ParseLegacyFormat(reader);
            EndTime = reader.ReadToSymbol('"');
        }
    }
}
