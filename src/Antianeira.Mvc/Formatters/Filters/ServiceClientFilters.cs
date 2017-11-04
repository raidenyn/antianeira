using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class ServiceClientFilters
    {
        public static string Request(Method method)
        {
            if (method.HasParams)
            {
                return method.Url.Parameters?.WriteToString();
            }

            if (method.HasBody)
            {
                return method.Request?.Type?.WriteToString();
            }

            return "void";
        }

        public static string Response(Method method)
        {
            return method.Response?.Type?.WriteToString() ?? "void";
        }

        public static string Method(Method method)
        {
            return method.HttpMethod.ToString();
        }
    }
}
