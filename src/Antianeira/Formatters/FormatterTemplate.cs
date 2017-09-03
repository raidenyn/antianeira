
using System;
using System.IO;
using DotLiquid;
using System.Collections.Generic;

namespace Antianeira.Formatters
{
    public class FormatterTemplate {
        private readonly Type[] _filters;

        public readonly Lazy<Template> MainTemplate;
        private readonly string _name;

        public FormatterTemplate(string name, Type[] filters) {
            _filters = filters;

            MainTemplate = new Lazy<Template>(() => LoadTemplate(name));
            _name = name;
        }

        public void Render(TextWriter writer, Func<object> variable) {
            MainTemplate.Value.Render(writer, new RenderParameters
            {
                Filters = _filters,
                LocalVariables = Hash.FromAnonymousObject(variable())
            });
        }

        public void Render(TextWriter writer, object variable)
        {
            MainTemplate.Value.Render(writer, new RenderParameters
            {
                Filters = _filters,
                LocalVariables = Hash.FromDictionary(new Dictionary<string, object> {[_name] = variable })
            });
        }

        private static Template LoadTemplate(string name)
        {
            var text = Templates.Templates.Load(name);
            return Template.Parse(text);
        }
    }
}
