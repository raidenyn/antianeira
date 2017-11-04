using System.Reflection;
using Antianeira.Schema;

namespace Antianeira.MetadataReader
{
    public interface IServiceMethodParameterMapper
    {
        ServiceMethodParameter MapServiceMethodParameter(ParameterInfo parameterInfo);
    }

    public class ServiceMethodParameterMapper
    {
        ServiceMethodParameter MapServiceMethodParameter(ParameterInfo parameterInfo);
    }
}
