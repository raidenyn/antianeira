using System;
using System.Collections.Generic;
using System.IO;

namespace Antianeira.Formatters
{
    public static class FormatterExtensions
    {
        public static string Render<TObject>(this IFormatterTemplates formatter, TObject variable) {
            using (var writer = new StreamWriter(new MemoryStream()))
            {
                formatter.Render(writer, variable);

                writer.Flush();
                writer.BaseStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(writer.BaseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string RenderMany<TObject>(this IFormatterTemplates formatter, IEnumerable<TObject> variables, string separator = "")
        {
            var lines = new List<string>();

            foreach (var variable in variables)
            {
                lines.Add(formatter.Render(variable));
            }

            return String.Join(separator, lines);
        }
    }
}
