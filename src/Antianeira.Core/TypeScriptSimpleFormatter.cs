using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Antianeira
{
    public class TypeScriptSimpleFormatter
    {
        private readonly Regex _lineRegex = new Regex(@"(.*)$", RegexOptions.Multiline);

        public string Format(string source)
        {
            var sb = new StringBuilder();

            var nesting = 0;

            foreach (Match match in _lineRegex.Matches(source))
            {
                var str = match.Groups[1].Value.Trim();
                if (!String.IsNullOrEmpty(str))
                {
                    var line = new NestingLine(str, nesting);

                    nesting += line.NestingShift;

                    sb.AppendLine(line.ToString());
                }
            }

            return sb.ToString();
        }
    }

    class NestingLine
    {
        public NestingLine(string line, int nesting)
        {
            Line = line;
            Nesting = nesting;
            NestingShift = Line.Count(s => s == '{' || s == '[' || s == '(') 
                           - Line.Count(s => s == '}' || s == ']' || s == ')');

            if (NestingShift < 0)
            {
                Nesting += NestingShift;
            }
        }

        public string Line { get; set; }

        public int Nesting { get; set; }

        public int Align { get; set; } = 4;

        public int NestingShift { get; set; }

        public override string ToString()
        {
            var ident = new String(' ', Nesting * Align);
            return ident + Line;
        }
    }
}
