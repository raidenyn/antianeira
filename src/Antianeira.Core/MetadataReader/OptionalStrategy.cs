using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IParameterOptionalStrategy
    {
        bool IsOptional([NotNull] ParameterInfo parameterInfo, [NotNull] TypeReferenceContext context);
    }

    public interface IPropertyOptionalStrategy
    {
        bool IsOptional([NotNull] PropertyInfo propertyInfo, [NotNull] TypeReferenceContext context);
    }

    public interface IReturnOptionalStrategy
    {
        bool IsOptional([NotNull] MethodInfo methodInfo, [NotNull] TypeReferenceContext context);
    }

    public class JetBrainsAttributeOptionalStrategy: 
        IParameterOptionalStrategy, 
        IPropertyOptionalStrategy,
        IReturnOptionalStrategy
    {
        public bool IsOptional(ParameterInfo parameterInfo, TypeReferenceContext context)
        {
            return IsOptional(parameterInfo.ParameterType, parameterInfo);
        }

        public bool IsOptional(PropertyInfo propertyInfo, TypeReferenceContext context)
        {
            return IsOptional(propertyInfo.PropertyType, propertyInfo);
        }

        public bool IsOptional(MethodInfo methodInfo, TypeReferenceContext context)
        {
            return IsOptional(methodInfo.ReturnType, methodInfo);
        }

        private bool IsOptional(Type type, ICustomAttributeProvider attributes)
        {
            if (type.IsGenericParameter)
            {
                if (attributes.IsDefined(typeof(ItemCanBeNullAttribute), inherit: true))
                {
                    return true;
                }
                if (attributes.IsDefined(typeof(ItemNotNullAttribute), inherit: true))
                {
                    return false;
                }
            }
            else
            {
                if (attributes.IsDefined(typeof(CanBeNullAttribute), inherit: true))
                {
                    return true;
                }
                if (attributes.IsDefined(typeof(NotNullAttribute), inherit: true))
                {
                    return false;
                }
            }
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
