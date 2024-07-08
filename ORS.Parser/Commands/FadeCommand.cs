using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORS.Parser.Commands
{
    public abstract class FadeCommand : BaseCommand
    {
        public string Type { get; private set; } = null!;

        protected override void Parse(StringReader reader)
        {
            Type = reader.ReadTo('\t');
        }
    }
}
