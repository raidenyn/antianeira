using System;
using System.Collections.Generic;
using System.Linq;
using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class ServiceMethodFilters
    {
        public static string Url(ServiceMethod method, string name = "url")
        {
            return method.Url.WriteToString(new MethodUrlRenderParams { UrlVariableName = name });
        }

        public static string Parameters(ServiceMethod method)
        {
            var @params = new List<string>();

            foreach (var parameter in method.Parameters)
            {
                var param = new Parameter(parameter.Name)
                {
                    Type = parameter.Type
                };

                @params.Add(param.WriteToString());
            }

            return String.Join(", ", @params);
        }

        public static string Response(ServiceMethod method)
        {
            return method.Response.Type.WriteToString();
        }

        public static string HttpMethod(ServiceMethod method)
        {
            return method.HttpMethod.ToString().ToUpper();
        }

        public static string BodyParamNames(ServiceMethod method)
        {
            var @params = method.Parameters.Where(p => p.PassOver == ParameterPassOver.Body).Select(p => p.Name);
            return String.Join(", ", @params);
        }

        public static string BodyParamsMerge(ServiceMethod method)
        {
            var bodies = method.Parameters.Where(p => p.PassOver == ParameterPassOver.Body).ToArray();

            if (bodies.Length == 1)
            {
                return bodies[0].Name;
            }

            var @params = bodies.Select(MergeWrapper);

            return "{" + String.Join(", ", @params) + "}";
        }

        /// <summary>
        /// Join all body parameters in one object with original parameters names
        /// </summary>
        public static string BodyParamsJoin(ServiceMethod method)
        {
            var bodies = method.Parameters.Where(p => p.PassOver == ParameterPassOver.Body).Select(p => p.Name).ToArray();

            if (bodies.Length == 1)
            {
                return bodies[0];
            }

            return "{" + String.Join(", ", bodies) + "}";
        }

        private static string MergeWrapper(ServiceMethodParameter parameter)
        {
            switch (parameter.Type.IsSimple)
            {
                case true: return parameter.Name;
                case false: return "..." + parameter.Name;
                default:
                    return $"...(typeof {parameter.Name} === 'object' ? ${parameter.Name} : {{ {parameter.Name} }})";
            }
        }
    }
}
