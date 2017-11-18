using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IMethodReponseMapper
    {
        [NotNull]
        ServiceMethodResponse MapMethodResponse([NotNull] MethodInfo methodInfo, [NotNull] MethodResponseContexts context);
    }

    public class MethodReponseMapper: IMethodReponseMapper
    {
        private readonly IReturnTypeMapper _returnTypeMapper;

        public MethodReponseMapper(IReturnTypeMapper returnTypeMapper)
        {
            _returnTypeMapper = returnTypeMapper;
        }

        public ServiceMethodResponse MapMethodResponse(MethodInfo methodInfo, MethodResponseContexts context)
        {
            var response = new ServiceMethodResponse();

            var typeContext = new TypeReferenceContext(context.Services.Definitions);

            response.SourceMethod = methodInfo;
            response.Type = _returnTypeMapper.GetReturnType(methodInfo, typeContext);

            return response;
        }
    }
}
