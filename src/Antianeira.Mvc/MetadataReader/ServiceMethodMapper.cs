using System.Linq;
using System.Reflection;
using Antianeira.MetadataReader.Comments;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IServiceMethodMapper
    {
        [CanBeNull]
        ServiceMethod GetMethod([NotNull] MethodInfo methodInfo, [NotNull] MethodContexts context);
    }

    public class DefaultServiceMethodMapper: IServiceMethodMapper
    {
        private readonly IMethodNameMapper _methodNameMapper;
        private readonly IMethodUrlMapper _methodUrlMapper;
        private readonly IHttpMethodMapper _httpMethodMapper;
        private readonly IServiceMethodParameterMapper _serviceMethodParameterMapper;
        private readonly IMethodReponseMapper _methodReponseMapper;
        private readonly ICommentsProvider _commentsProvider;

        public DefaultServiceMethodMapper(
            IMethodNameMapper methodNameMapper, 
            IMethodUrlMapper methodUrlMapper, 
            IHttpMethodMapper httpMethodMapper, 
            IServiceMethodParameterMapper serviceMethodParameterMapper, 
            IMethodReponseMapper methodReponseMapper, 
            ICommentsProvider commentsProvider)
        {
            _methodNameMapper = methodNameMapper;
            _methodUrlMapper = methodUrlMapper;
            _httpMethodMapper = httpMethodMapper;
            _serviceMethodParameterMapper = serviceMethodParameterMapper;
            _methodReponseMapper = methodReponseMapper;
            _commentsProvider = commentsProvider;
        }


        public ServiceMethod GetMethod(MethodInfo methodInfo, MethodContexts context)
        {
            var method = new ServiceMethod(_methodNameMapper.GetMethodName(methodInfo));

            var parameterContext = new MethodParameterContexts(context.Services);
            var responseContext = new MethodResponseContexts(context.Services);

            method.SourceMethod = methodInfo;

            method.HttpMethod = _httpMethodMapper.MapHttpMethod(methodInfo);

            method.Parameters =
                (from parameterInfo in methodInfo.GetParameters()
                    let parameter = _serviceMethodParameterMapper.MapServiceMethodParameter(parameterInfo, parameterContext)
                    where parameter != null
                    select parameter).ToList();

            method.Url = _methodUrlMapper.MapMethodUrl(methodInfo, method.Parameters);

            method.Response = _methodReponseMapper.MapMethodResponse(methodInfo, responseContext);

            method.Comment = _commentsProvider.GetComment(methodInfo);

            return method;
        }
    }
}
