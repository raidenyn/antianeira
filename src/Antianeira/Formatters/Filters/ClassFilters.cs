using System;
using Antianeira.Schema;
using System.Linq;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class ClassFilters
    {
        public static string Class(Class @class)
        {
            var sb = new StringBuilder();

            if (@class.IsDeclared)
            {
                sb.Append("declare ");
            }

            if (@class.IsExported)
            {
                sb.Append("export ");
            }

            if (@class.IsAbstract)
            {
                sb.Append("abstract ");
            }

            sb.Append("class ");

            sb.Append(@class.Name);

            if (@class.BaseClass != null)
            {
                sb.Append(" extends ").Append(@class.BaseClass.Name);
            }

            if (@class.Interfaces.Any())
            {
                sb.Append(" implemented ").Append(String.Join(", ", @class.Interfaces.Select(i => i.Name)));
            }

            return sb.ToString();
        }

        public static string Properties(Class @class)
        {
            return FormatterTemplates.Current.RenderMany(@class.Properties);
        }
    }
}
