using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IReturnTypeMapper
    {
        [NotNull]
        ReturnType GetReturnType([NotNull] MethodInfo methodInfo, [NotNull] TypeReferenceContext context);
    }

    internal class DefaultReturnTypeMapper : IReturnTypeMapper
    {
        private readonly ITypeReferenceMapper _typeReference;
        private readonly IReturnOptionalStrategy _optionalStrategy;
        private readonly IReturnNullableStrategy _nullableStrategy;

        public DefaultReturnTypeMapper(
            ITypeReferenceMapper typeReference,
            IReturnOptionalStrategy optionalStrategy,
            IReturnNullableStrategy nullableStrategy
        )
        {
            _optionalStrategy = optionalStrategy;
            _typeReference = typeReference;
            _nullableStrategy = nullableStrategy;
        }

        public virtual ReturnType GetReturnType(MethodInfo methodInfo, TypeReferenceContext context)
        {
            TypeReference typeReference = GetTypeReference(methodInfo.ReturnType, context);

            var result = new ReturnType
            {
                IsOptional = _optionalStrategy.IsOptional(methodInfo, context),
                IsNullable = _nullableStrategy.IsNullable(methodInfo, context)
            };

            if (typeReference != null)
            {
                result.Types.Add(typeReference);
            }
            
            return result;
        }

        [CanBeNull]
        protected virtual TypeReference GetTypeReference([NotNull] Type type, [NotNull] TypeReferenceContext context)
        {
            type = GetReturnType(type);

            if (type != null)
            {
                return _typeReference.GetTypeReference(type, context) ?? new AnyType();
            }

            return null;
        }

        [CanBeNull]
        protected Type GetReturnType([NotNull] Type type)
        {
            if (typeof(Task).IsAssignableFrom(type))
            {
                type = type.GetGenericArguments().FirstOrDefault();
            }

            if (type == typeof(void))
            {
                return null;
            }

            return type;
        }
    }
}
