using System.Reflection;
using Antianeira.Utils;

namespace Antianeira.MetadataReader
{
    public interface IEnumFieldMapper
    {
        string GetFieldName(FieldInfo field);
    }

    public class CamelCaseEnumFieldMapper : IEnumFieldMapper
    {
        public string GetFieldName(FieldInfo field)
        {
            return StringUtils.ToCamelCase(field.Name);
        }
    }
}
