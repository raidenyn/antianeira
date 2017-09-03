using Antianeira.Schema;
using System;
using System.Collections.Generic;

namespace Antianeira.Formatters.Filters
{
    public static class DefinitionsFilters
    {
        public static string Classes(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.Classes);
        }

        public static string Interfaces(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.Interfaces);
        }

        public static string Types(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.Types);
        }

        public static string Enums(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.Enums);
        }

        public static string Clients(Definitions @definitions)
        {
            return FormatterTemplates.Current.RenderMany(@definitions.ServiceClients);
        }

        public static string Names(IEnumerable<ITsType> types)
        {
            return String.Join(", ", types);
        }
    }
}
