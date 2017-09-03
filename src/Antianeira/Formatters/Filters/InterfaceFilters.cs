using System;
using System.Linq;
using Antianeira.Schema;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class InterfaceFilters
    {
        public static string Interface(Interface @interface)
        {
            var sb = new StringBuilder();

            if (@interface.IsDeclared)
            {
                sb.Append("declare ");
            }

            if (@interface.IsExported)
            {
                sb.Append("export ");
            }

            sb.Append("interface ");

            sb.Append(@interface.Name);

            if (@interface.Interfaces.Any())
            {
                sb.Append(" extends ").Append(String.Join(", ", @interface.Interfaces.Select(i => i.Name)));
            }

            return sb.ToString();
        }

        public static string Properties(Interface @interface)
        {
            return FormatterTemplates.Current.RenderMany(@interface.Properties);
        }

        public static string Methods(Interface @interface)
        {
            return FormatterTemplates.Current.RenderMany(@interface.Methods);
        }
    }
}
