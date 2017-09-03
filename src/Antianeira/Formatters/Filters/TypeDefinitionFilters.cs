using System;
using System.Linq;
using Antianeira.Schema;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class TypeDefinitionFilters
    {
        public static string Type(TypeDefinition type)
        {
            var sb = new StringBuilder();

            if (type.IsDeclared)
            {
                sb.Append("declare ");
            }

            if (type.IsExported)
            {
                sb.Append("export ");
            }

            sb.Append("type ");
            sb.Append(type.Name);

            return sb.ToString();
        }

        public static string Types(TypeDefinition type)
        {
            if (type.Types.Count == 0)
            {
                return "null";
            }

            return String.Join(" | ", type.Types.Select(t=> $"'{t}'"));
        }
    }
}
