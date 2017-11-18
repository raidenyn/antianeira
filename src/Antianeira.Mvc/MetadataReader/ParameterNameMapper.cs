using System.Reflection;
using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IParameterNameMapper
    {
        [NotNull]
        string MapParameterName([NotNull] ParameterInfo paramter);
    }

    public class CamelCaseParameterNameMapper: IParameterNameMapper
    {
        public string MapParameterName(ParameterInfo paramter)
        {
            return StringUtils.ToCamelCase(paramter.Name);
        }
    }
}
