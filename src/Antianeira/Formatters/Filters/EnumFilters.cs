using System;
using Antianeira.Schema;
using Enum = Antianeira.Schema.Enum;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class EnumFilters
    {
        public static string Enum(Enum @enum)
        {
            var sb = new StringBuilder();

            if (@enum.IsDeclared)
            {
                sb.Append("declare ");
            }

            if (@enum.IsExported)
            {
                sb.Append("export ");
            }

            sb.Append(@enum.Name);

            return sb.ToString();
        }

        public static string Fields(Enum @enum)
        {
            return FormatterTemplates.Current.RenderMany(@enum.Fields);
        }
    }
}
