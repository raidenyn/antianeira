using System.Collections.Generic;

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
    }
}
