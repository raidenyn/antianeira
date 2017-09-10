using System.Collections.Generic;
using System.Text;

namespace Antianeira
{
    public interface IWriter
    {
        IWriter Append(string text);
    }

    public interface IWritable
    {
        void Write(IWriter writer);
    }

    public static class WriterExtensions
    {
        public static IWriter Join(this IWriter writer, string separator, IEnumerable<IWritable> writables) {
            var counter = 0;
            foreach (var writable in writables) {
                if (counter > 0) {
                    writer.Append(separator);
                }
                writable.Write(writer);
            }
            return writer;
        }

        public static string WriteToString(this IWritable writable)
        {
            var writer = new StringWriter();

            writable.Write(writer);

            return writer.ToString();
        }
    }

    public class StringWriter : IWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public IWriter Append(string text)
        {
            _sb.Append(text);
            return this;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
