
using System;
using System.IO;
using DotLiquid;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Antianeira.Formatters
{
    public class FormatterTemplate {
        private readonly Type[] _filters;

        public readonly Lazy<Template> MainTemplate;
        private readonly string _name;

        public FormatterTemplate(Assembly assembly, string name, Type[] filters) {
            _filters = filters;

            MainTemplate = new Lazy<Template>(() => LoadTemplate(assembly, name));
            _name = name;
        }

        public void Render(TextWriter writer, Func<object> variable) {
            MainTemplate.Value.Render(writer, new RenderParameters(CultureInfo.InvariantCulture)
            {
                Filters = _filters,
                LocalVariables = Hash.FromAnonymousObject(variable())
            });
        }

        public void Render(TextWriter writer, object variable)
        {
            MainTemplate.Value.Render(writer, new RenderParameters(CultureInfo.InvariantCulture)
            {
                Filters = _filters,
                LocalVariables = Hash.FromDictionary(new Dictionary<string, object> {[_name] = variable })
            });
        }

        private static Template LoadTemplate(Assembly assembly, string name)
        {
            var text = Templates.Load(assembly, name);
            return Template.Parse(text);
        }
    }
}
