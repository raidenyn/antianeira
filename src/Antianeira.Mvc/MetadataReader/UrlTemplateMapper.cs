using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Antianeira.MetadataReader
{
    public interface IUrlTemplateMapper
    {
        string MapUrlTemplate(MethodInfo methodInfo);
    }

    public class AttributeUrlTemplateMapper: IUrlTemplateMapper
    {
        public string MapUrlTemplate(MethodInfo methodInfo)
        {
            var classRouteAttr = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<RouteAttribute>();
            var methodRouteAttr = methodInfo.GetCustomAttribute<RouteAttribute>();

            if (classRouteAttr == null && methodRouteAttr == null)
            {
                return null;
            }

            return classRouteAttr?.Template + "/" + methodRouteAttr?.Template;
        }
    }
}
