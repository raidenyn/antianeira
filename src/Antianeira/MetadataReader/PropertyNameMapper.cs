using System.Reflection;
using Antianeira.Utils;

namespace Antianeira.MetadataReader
{
    public interface IPropertyNameMapper
    {
        string GetPropertyName(PropertyInfo property);
    }

    public class CamelCasePropertyTypeName : IPropertyNameMapper
    {
        public string GetPropertyName(PropertyInfo property)
        {
            return StringUtils.ToCamelCase(property.Name);
        }
    }
}