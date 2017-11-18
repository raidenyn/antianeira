using System;
using System.Collections.Generic;
using System.Text;
using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira
{
    public interface IWriter
    {
        [NotNull]
        WritingParameters Parameters { get; }

        IWriter Append(string text);
    }

    public interface IWritable
    {
        void Write(IWriter writer);
    }

    public static class WriterExtensions
    {
        public static IWriter Join(this IWriter writer, string separator, [NotNull, ItemNotNull] IEnumerable<IWritable> writables) {
            var counter = 0;
            foreach (var writable in writables) {
                if (counter > 0) {
                    writer.Append(separator);
                }
                writable.Write(writer);
                counter++;
            }
            return writer;
        }

        public static string WriteToString([NotNull] this IWritable writable, params object[] @params)
        {
            var writer = new StringWriter();

            foreach (var param in @params) {
                writer.Parameters.Set(param);
            }

            writable.Write(writer);

            return writer.ToString();
        }
    }

    public class StringWriter : IWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public WritingParameters Parameters { get; } = new WritingParameters();

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

    public class WritingParameters
    {
        private readonly Dictionary<Type, object> _params = new Dictionary<Type, object>();

        public void Set(object @params)
        {
            _params[@params.GetType()] = @params;
        }

        public void Set<T>(T @params)
            where T : class
        {
            _params[typeof(T)] = @params;
        }

        public T Get<T>(Func<T> @default)
            where T: class
        {
            return _params.SafeGet(typeof(T)) as T ?? @default();
        }
    }
}
