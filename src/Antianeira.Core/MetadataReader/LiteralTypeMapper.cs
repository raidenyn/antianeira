using System.Reflection;
using Antianeira.Utils;

namespace Antianeira.MetadataReader
{
    public interface ILiteralTypeMapper
    {
        string GetLiteralType(FieldInfo field);
    }

    public class CamelCaseLiteralTypeMapper : ILiteralTypeMapper
    {
        public string GetLiteralType(FieldInfo field)
        {
            return StringUtils.ToCamelCase(field.Name);
        }
    }
}
