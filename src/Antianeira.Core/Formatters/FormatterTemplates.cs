using System;
using System.Collections.Generic;
using System.IO;

namespace Antianeira.Formatters
{
    public interface IFormatterTemplates
    {
        void Render<TObject>(TextWriter writer, TObject variable);

        void Register<TObject>(FormatterTemplate formatter);
    }

    public class FormatterTemplates : IFormatterTemplates
    {
        private static readonly IDictionary<Type, FormatterTemplate> Templates = new Dictionary<Type, FormatterTemplate>();

        public static IFormatterTemplates Current { get; } = new FormatterTemplates();

        private FormatterTemplates() { }

        public void Register<TObject>(FormatterTemplate formatter)
        {
            Templates[typeof(TObject)] = formatter;
        }

        public void Render<TObject>(TextWriter writer, TObject variable)
        {
            GetTemplate<TObject>().Render(writer, variable);
        }

        private FormatterTemplate GetTemplate<TObject>()
        {
            return Templates[typeof(TObject)];
        }
    }
}
