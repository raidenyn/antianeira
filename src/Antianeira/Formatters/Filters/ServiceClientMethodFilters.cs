using Antianeira.Schema.Api;

namespace Antianeira.Formatters.Filters
{
    public static class ServiceClientMethodFilters
    {
        public static string Request(Method method)
        {
            if (method.HasParams) {
                return method.Url.Parameters.Structure.Name;
            }

            if (method.HasBody)
            {
                return method.Request.Type.Name;
            }

            return "void";
        }

        public static string Response(Method method)
        {
            return method.Response?.Type?.Name ?? "void";
        }

        public static string Url(Method method)
        {
            return method.Url.Path;
        }

        public static string Method(Method method)
        {
            return method.HttpMethod.ToString().ToLowerInvariant();
        }
    }
}
