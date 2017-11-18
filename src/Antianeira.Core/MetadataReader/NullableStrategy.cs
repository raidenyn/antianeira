using JetBrains.Annotations;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IParameterNullableStrategy
    {
        bool IsNullable([NotNull] ParameterInfo parameterInfo, [NotNull] TypeReferenceContext context);
    }

    public interface IPropertyNullableStrategy
    {
        bool IsNullable([NotNull] PropertyInfo propertyInfo, [NotNull] TypeReferenceContext context);
    }

    public interface IReturnNullableStrategy
    {
        bool IsNullable([NotNull] MethodInfo methodInfo, [NotNull] TypeReferenceContext context);
    }

    public class NeverNullableStrategy: 
        IParameterNullableStrategy, 
        IPropertyNullableStrategy,
        IReturnNullableStrategy
    {
        public bool IsNullable(ParameterInfo parameterInfo, TypeReferenceContext context)
        {
            return false;
        }

        public bool IsNullable(PropertyInfo propertyInfo, TypeReferenceContext context)
        {
            return false;
        }

        public bool IsNullable(MethodInfo methodInfo, TypeReferenceContext context)
        {
            return false;
        }
    }
}
