using System;
using System.IO;
using System.Text;

namespace Antianeira
{
    public class AlignStreamWriter : StreamWriter
    {
        private int _deep = 0;
        private readonly int _align = 4;
        private bool _oneLine = true;

        public AlignStreamWriter(Stream stream) : base(stream)
        {
        }

        public AlignStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public override void Write(string value)
        {
            var sb = new StringBuilder(value.Length);
            foreach (var symbol in value)
            {
                string s = symbol.ToString();
                switch (symbol)
                {
                    case '{':
                    {
                        _oneLine = true;
                        _deep++;
                        break;
                    }
                    case '}':
                    {
                        _deep--;
                        if (!_oneLine)
                        {
                            var align = new String(' ', _deep * _align);
                            s = '\n' + align + '}';
                        }
                        break;
                    }
                    case '\n':
                    {
                        _oneLine = false;
                        var align = new String(' ', _deep * _align);
                        s = symbol + align;
                        break;
                    }
                }
                sb.Append(s);
            }

            base.Write(sb.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Flush();
            }
            base.Dispose(disposing);
        }
    }
}
