using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IParameterTypeMapper
    {
        [NotNull]
        ParameterType GetParameterType([NotNull] ParameterInfo parameterInfo, [NotNull] TypeReferenceContext context);
    }

    internal class DefaultParameterTypeMapper : IParameterTypeMapper
    {
        private readonly IParameterOptionalStrategy _optionalStrategy;
        private readonly ITypeReferenceMapper _typeReference;
        private readonly IParameterNullableStrategy _nullableStrategy;

        public DefaultParameterTypeMapper(
            ITypeReferenceMapper typeReference,
            IParameterOptionalStrategy optionalStrategy,
            IParameterNullableStrategy nullableStrategy
        ) {
            _optionalStrategy = optionalStrategy;
            _typeReference = typeReference;
            _nullableStrategy = nullableStrategy;
        }

        public ParameterType GetParameterType(ParameterInfo parameterInfo, TypeReferenceContext context)
        {
            TypeReference typeReference = _typeReference.GetTypeReference(parameterInfo.ParameterType, context) ?? new AnyType();

            return new ParameterType
            {
                IsOptional = _optionalStrategy.IsOptional(parameterInfo, context),
                IsNullable = _nullableStrategy.IsNullable(parameterInfo, context),
                Types = { typeReference }
            };
        }
    }
}
