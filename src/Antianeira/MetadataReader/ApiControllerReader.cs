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
        public void Read([NotNull] Assembly assembly, Definitions definitions) {
            var controllers = from type in assembly.GetTypes()
                              where typeof(Controller).IsAssignableFrom(type)
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
            var method = new Method();

            method.Name = _settings.MethodNameMapper.GetMethodName(methodInfo);

            var httpMethodAttr = methodInfo.GetCustomAttribute<HttpMethodAttribute>();
            method.HttpMethod = new HttpMethod(httpMethodAttr?.HttpMethods.FirstOrDefault() ?? HttpMethod.Get.Method);

            var classRouteAttr = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<RouteAttribute>();
            var methodRouteAttr = methodInfo.GetCustomAttribute<RouteAttribute>();

            if (classRouteAttr == null && methodRouteAttr == null) {
                return null;
            }

            method.Url = new MethodUrl();
            method.Url.Path = classRouteAttr?.Template + "/" + methodRouteAttr?.Template;

            var parameters = ParametersReader.ReadFrom(methodInfo);

            var bodyParameter = parameters.FirstOrDefault(p=>p.PassOver == ParameterPassOver.Body);
            if (bodyParameter != null)
            {
                method.Request = new MethodRequest
                {
                    Type = _settings.DefinitionsMapper.ConvertType(bodyParameter.Type.GetTypeInfo(), definitions)
                };
            }

            var queryParameters = parameters.Where(p => p.PassOver == ParameterPassOver.Query).ToArray();
            if (queryParameters.Any())
            {
                var structureParam = queryParameters.FirstOrDefault(p=>!p.Type.IsPrimitive && typeof(string) != p.Type);

                if (structureParam != null)
                {
                    method.Url.Parameters.Structure = _settings.DefinitionsMapper.ConvertType(structureParam.Type.GetTypeInfo(), definitions);
                }
                else {
                    var singleParameters = queryParameters;
                    var name = "I" + client.Name + methodInfo.Name + "Request";
                    method.Url.Parameters.Structure = definitions.Interfaces.GetOrCreate(name, () => {
                        var @interface = new Interface(name);
                        @interface.IsExported = true;

                        foreach (var param in singleParameters)
                        {
                            var property = new InterfaceProperty(param.Name);

                            property.Type = _settings.PropertyTypeMapper.GetPropertyType(param.Type.GetTypeInfo(), definitions);

                            @interface.Properties.Add(property);
                        }

                        return @interface;
                    });
                }
            }

            if (methodInfo.ReturnType != null) {
                method.Response = new MethodResponse
                {
                    Type = _settings.DefinitionsMapper.ConvertType(methodInfo.ReturnType.GetTypeInfo(), definitions)
                };
            }

            return method;
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

    public class ParametersReader {
        public static List<Parameter> ReadFrom(MethodInfo method) {
            return method.GetParameters().Select(ReadFrom).ToList();
        }

        public static Parameter ReadFrom(ParameterInfo parameter)
        {
            var result = new Parameter();

            result.Name = parameter.Name;
            result.Type = parameter.ParameterType;

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
}
