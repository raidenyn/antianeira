using System.Linq;
using System.Net.Http;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Antianeira.MetadataReader
{
    public interface IHttpMethodMapper
    {
        [NotNull]
        HttpMethod MapHttpMethod([NotNull] MethodInfo methodInfo);
    }

    public class AttributeHttpMethodMapper: IHttpMethodMapper
    {
        public HttpMethod MapHttpMethod(MethodInfo methodInfo)
        {
            var httpMethodAttr = methodInfo.GetCustomAttribute<HttpMethodAttribute>();
            return new HttpMethod(httpMethodAttr?.HttpMethods.FirstOrDefault() ?? HttpMethod.Get.Method);
        }
    }
}
