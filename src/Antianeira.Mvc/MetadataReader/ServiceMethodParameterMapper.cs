using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IServiceMethodParameterMapper
    {
        [CanBeNull]
        ServiceMethodParameter MapServiceMethodParameter([NotNull] ParameterInfo parameterInfo, [NotNull] MethodParameterContexts context);
    }

    public class ServiceMethodParameterMapper: IServiceMethodParameterMapper
    {
        private readonly IParameterPassOverMapper _parameterPassOverMapper;
        private readonly IParameterNameMapper _parameterNameMapper;
        private readonly IParameterTypeMapper _parameterTypeMapper;

        public ServiceMethodParameterMapper(
            IParameterPassOverMapper parameterPassOverMapper, 
            IParameterNameMapper parameterNameMapper,
            IParameterTypeMapper parameterTypeMapper)
        {
            _parameterPassOverMapper = parameterPassOverMapper;
            _parameterNameMapper = parameterNameMapper;
            _parameterTypeMapper = parameterTypeMapper;
        }

        public ServiceMethodParameter MapServiceMethodParameter(ParameterInfo parameterInfo, MethodParameterContexts context)
        {
            var name = _parameterNameMapper.MapParameterName(parameterInfo);
            var parameter = new ServiceMethodParameter(name);

            var propertyContext = new TypeReferenceContext(context.Services.Definitions);

            parameter.Source = parameterInfo;
            parameter.PassOver = _parameterPassOverMapper.MapPassOver(parameterInfo);
            parameter.Type = _parameterTypeMapper.GetParameterType(parameterInfo, propertyContext);

            return parameter;
        }
    }
}
