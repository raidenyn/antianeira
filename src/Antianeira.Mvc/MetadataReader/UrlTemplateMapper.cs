using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IUrlTemplateMapper
    {
        [NotNull]
        string MapUrlTemplate([NotNull] MethodInfo methodInfo);
    }

    public class AttributeUrlTemplateMapper: IUrlTemplateMapper
    {
        public string MapUrlTemplate(MethodInfo methodInfo)
        {
            var classRouteAttr = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<RouteAttribute>();
            var methodRouteAttr = methodInfo.GetCustomAttribute<RouteAttribute>();

            return classRouteAttr?.Template + "/" + methodRouteAttr?.Template;
        }
    }
}
