using Antianeira.Schema;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class InterfacePropertyFilters
    {
        public static string Property(InterfaceProperty property)
        {
            var sb = new StringBuilder();

            sb.Append(property.Name);

            if (property.Type.IsOptional)
            {
                sb.Append("?");
            }

            return sb.ToString();
        }
    }
}
