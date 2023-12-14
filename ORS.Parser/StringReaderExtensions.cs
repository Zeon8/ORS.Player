using System.Text;

namespace ORS.Parser
{
    internal static class StringReaderExtensions
    {
        public static void SkipToSymbol(this StringReader reader, char value)
        {
            int symbol;
            do
            {
                symbol = reader.Read();
            }
            while (symbol != -1 && symbol != value);
        }

        public static string ReadToSymbol(this StringReader reader, char value, bool ignoreSpaces = true)
        {
            var streamBuilder = new StringBuilder();
            int symbol = reader.Read();
            while (symbol != -1 && symbol != value)
            {
                if (ignoreSpaces && symbol == ' ')
                {
                    symbol = reader.Read();
                    continue;
                }

                streamBuilder.Append((char)symbol);
                symbol = reader.Read();
            }
            return streamBuilder.ToString();
        }
    }
}
