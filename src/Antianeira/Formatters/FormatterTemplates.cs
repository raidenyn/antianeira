using Antianeira.Formatters.Filters;
using Antianeira.Schema;
using Antianeira.Schema.Api;
using System;
using System.Collections.Generic;
using System.IO;

namespace Antianeira.Formatters
{
    public interface IFormatterTemplates
    {
        void Render<TObject>(TextWriter writer, TObject variable);
    }

    public class FormatterTemplates : IFormatterTemplates
    {
        private static readonly IDictionary<Type, FormatterTemplate> _templates = new Dictionary<Type, FormatterTemplate>
        {
            [typeof(Definitions)] =
              new FormatterTemplate("definitions", new[] { typeof(DefinitionsFilters) }),
            [typeof(ServiceClient)] =
              new FormatterTemplate("client", new[] { typeof(ServiceClientFilters), typeof(ServiceClientMethodFilters) }),
        };

        private FormatterTemplates() { }

        public static IFormatterTemplates Current { get; set; } = new FormatterTemplates();

        public FormatterTemplate GetTemplate<TObject>()
        {
            return _templates[typeof(TObject)];
        }

        public void Render<TObject>(TextWriter writer, TObject variable)
        {
            GetTemplate<TObject>().Render(writer, variable);
        }
    }
}
