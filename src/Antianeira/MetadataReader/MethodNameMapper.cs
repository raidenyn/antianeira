using System.Reflection;
using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IMethodNameMapper
    {
        [NotNull]
        string GetMethodName([NotNull] MethodInfo method);
    }

    public class CamelCaseMethodName : IMethodNameMapper
    {
        public string GetMethodName(MethodInfo method)
        {
            return StringUtils.ToCamelCase(method.Name);
        }
    }
}