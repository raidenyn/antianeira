using Antianeira.Schema;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class TypeFilters
    {
        public static string Type(TypeReference type)
        {
            var sb = new StringBuilder();

            sb.Append(type.Name);

            if (type.IsNullable)
            {
                sb.Append(" | null");
            }

            return sb.ToString();
        }
    }
}
