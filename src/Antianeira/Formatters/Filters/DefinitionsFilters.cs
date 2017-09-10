using Antianeira.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Antianeira.Formatters.Filters
{
    public static class DefinitionsFilters
    {
        public static string Classes(Definitions @definitions)
        {
            var writer = new StringWriter();

            foreach (var @class in @definitions.Classes) {
                @class.Write(writer);
            }

            return writer.ToString();
        }

        public static string Interfaces(Definitions @definitions)
        {
            var writer = new StringWriter();

            foreach (var @interface in @definitions.Interfaces)
            {
                @interface.Write(writer);
            }

            return writer.ToString();
        }

        public static string Types(Definitions @definitions)
        {
            var writer = new StringWriter();

            foreach (var type in @definitions.Types)
            {
                type.Write(writer);
            }

            return writer.ToString();
        }

        public static string Enums(Definitions @definitions)
        {
            var writer = new StringWriter();

            foreach (var @enum in @definitions.Enums)
            {
                @enum.Write(writer);
            }

            return writer.ToString();
        }

        public static string Clients(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.ServiceClients);
        }

        public static string Names(IEnumerable<ITsType> types)
        {
            return String.Join(",\n", types.Select(t => t.Name));
        }
    }
}
