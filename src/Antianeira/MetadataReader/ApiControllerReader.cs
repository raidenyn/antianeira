using Antianeira.Schema;
using Antianeira.Schema.Api;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Antianeira.MetadataReader
{
    public class ApiControllerLoader
    {
        private readonly MappingSettings _settings;

        public ApiControllerLoader(MappingSettings settings = null)
        {
            _settings = settings ?? new MappingSettings();
        }

        [NotNull]
        public void Read([NotNull] Assembly assembly, Definitions definitions, ApiControllerLoaderOptions options = null) {
            options = options ?? new ApiControllerLoaderOptions();

            var controllers = from type in assembly.GetTypes()
                              where typeof(Controller).IsAssignableFrom(type)
                                    && Glob.Glob.IsMatch(type.FullName, options.TypeFilter)
                              select type;

            foreach (var controller in controllers) {
                AddController(controller, definitions);
            }
        }

        [NotNull]
        public void AddController([NotNull] Type controller, [NotNull] Definitions definitions)
        {
            var name = controller.Name.Replace("Controller", "Client");

            var methods = (from method in controller.GetMethods()
                           where !typeof(IActionResult).IsAssignableFrom(method.ReturnType)
                           select method).ToArray();

            if (methods.Length == 0) {
                return;
            }

            definitions.ServiceClients.GetOrCreate(name, () => {
                var client = new ServiceClient(name)
                {
                    IsExported = true
                };

                foreach (var methodInfo in methods)
                {
                    var method = GetMethod(methodInfo, client, definitions);

                    if (method != null) {
                        client.Methods.Add(method);
                    }
                }

                return client;
            });
        }

        [CanBeNull]
        private Method GetMethod([NotNull] MethodInfo methodInfo, [NotNull] ServiceClient client, [NotNull] Definitions definitions)
        {
            var method = new Method
            {
                Name = _settings.MethodNameMapper.GetMethodName(methodInfo)
            };

            var httpMethodAttr = methodInfo.GetCustomAttribute<HttpMethodAttribute>();
            method.HttpMethod = new HttpMethod(httpMethodAttr?.HttpMethods.FirstOrDefault() ?? HttpMethod.Get.Method);

            var classRouteAttr = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<RouteAttribute>();
            var methodRouteAttr = methodInfo.GetCustomAttribute<RouteAttribute>();

            if (classRouteAttr == null && methodRouteAttr == null) {
                return null;
            }

            method.Url = new MethodUrl
            {
                Path = classRouteAttr?.Template + "/" + methodRouteAttr?.Template
            };

            var parameters = ParametersReader.ReadFrom(methodInfo);

            var bodyParameter = parameters.Find(p => p.PassOver == ParameterPassOver.Body);
            if (bodyParameter != null)
            {
                method.Request = new MethodRequest
                {
                    Type = _settings.PropertyTypeMapper.GetPropertyType(bodyParameter.Type.GetTypeInfo(), new PropertyTypeContext(definitions))
                };
            }

            var queryParameters = parameters.Where(p => p.PassOver == ParameterPassOver.Query).ToArray();
            if (queryParameters.Length > 0)
            {
                var structureParam = Array.Find(queryParameters, p => !p.Type.IsPrimitive && typeof(string) != p.Type);

                if (structureParam != null)
                {
                    method.Url.Parameters.Structure = _settings.DefinitionsMapper.ConvertType(structureParam.Type.GetTypeInfo(), definitions);
                }
                else {
                    var singleParameters = queryParameters;
                    var name = "I" + client.Name + methodInfo.Name + "Request";
                    method.Url.Parameters.Structure = definitions.Interfaces.GetOrCreate(name, () => {
                        var @interface = new Interface(name)
                        {
                            IsExported = true
                        };

                        foreach (var param in singleParameters)
                        {
                            var property = new InterfaceProperty(param.Name)
                            {
                                Type = _settings.PropertyTypeMapper.GetPropertyType(param.Type.GetTypeInfo(), new PropertyTypeContext(definitions))
                            };

                            @interface.Properties.Add(property);
                        }

                        return @interface;
                    });
                }
            }

            var returnType = GetReturnType(methodInfo);
            if (returnType != null) {
                method.Response = new MethodResponse
                {
                    Type = _settings.PropertyTypeMapper.GetPropertyType(returnType.GetTypeInfo(), new PropertyTypeContext(definitions))
                };
            }

            return method;
        }

        [CanBeNull]
        private Type GetReturnType([NotNull] MethodInfo methodInfo) {
            var type = methodInfo.ReturnType;

            if (typeof(Task).IsAssignableFrom(type))
            {
                return type.GetGenericArguments().FirstOrDefault();
            }

            return type;
        }
    }

    public enum ParameterPassOver {
        Query,

        Body,

        Header,

        Route,

        Service,

        Form
    }

    public class Parameter {
        public ParameterPassOver PassOver;

        public string Name;

        public Type Type;
    }

    public static class ParametersReader {
        public static List<Parameter> ReadFrom(MethodInfo method) {
            return method.GetParameters().Select(ReadFrom).ToList();
        }

        public static Parameter ReadFrom(ParameterInfo parameter)
        {
            var result = new Parameter
            {
                Name = parameter.Name,
                Type = parameter.ParameterType
            };

            if (parameter.GetCustomAttribute<FromBodyAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Body;
            }
            else if (parameter.GetCustomAttribute<FromFormAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Form;
            }
            else if (parameter.GetCustomAttribute<FromHeaderAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Header;
            }
            else if (parameter.GetCustomAttribute<FromQueryAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Query;
            }
            else if (parameter.GetCustomAttribute<FromRouteAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Route;
            }
            else if (parameter.GetCustomAttribute<FromServicesAttribute>() != null)
            {
                result.PassOver = ParameterPassOver.Service;
            }
            else
            {
                result.PassOver = ParameterPassOver.Query;
            }

            return result;
        }
    }

    public class ApiControllerLoaderOptions {
        /// <summary>
        /// Glob filter
        /// </summary>
        public string TypeFilter { get; set; } = "*";
    }
}
