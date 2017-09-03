using Antianeira.Schema;
using System.Text;

namespace Antianeira.Formatters.Filters
{
    public static class ClassPropertyFilters
    {
        public static string Property(ClassProperty property)
        {
            var sb = new StringBuilder();

            switch (property.AccessLevel)
            {
                case PropertyAccessLevel.Private:
                    sb.Append("private ");
                    break;
                case PropertyAccessLevel.Protected:
                    sb.Append("protected ");
                    break;
                case PropertyAccessLevel.Public:
                    sb.Append("public ");
                    break;
            }

            if (property.IsStatic)
            {
                sb.Append("static ");
            }
            else
            {
                if (property.IsAbstract)
                {
                    sb.Append("abstract ");
                }
            }

            sb.Append(property.Name);

            if (property.Type.IsOptional)
            {
                sb.Append("?");
            }

            return sb.ToString();
        }
    }
}
