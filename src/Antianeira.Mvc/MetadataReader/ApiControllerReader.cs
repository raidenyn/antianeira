using Antianeira.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Antianeira.Schema.MethodUrl;

namespace Antianeira.MetadataReader
{
    public interface IApiControllerReader
    {
        void Read([NotNull] Assembly assembly, [NotNull] ServicesDefinitions serivces, ApiControllerLoaderOptions options = null);
    }

    public class ApiControllerReader: IApiControllerReader
    {
        private readonly IMethodNameMapper _methodNameMapper;
        private readonly IPropertyTypeMapper _propertyTypeMapper;
        private readonly ITypeReferenceMapper _typeReferenceMapper;
        private readonly IMethodUrlMapper _methodUrlMapper;
        private readonly IHttpMethodMapper _httpMethodMapper;

        public ApiControllerReader(
            IMethodNameMapper methodNameMapper,
            IPropertyTypeMapper propertyTypeMapper,
            ITypeReferenceMapper typeReferenceMapper,
            IMethodUrlMapper methodUrlMapper,
            IHttpMethodMapper httpMethodMapper
        ) {
            _methodNameMapper = methodNameMapper;
            _propertyTypeMapper = propertyTypeMapper;
            _typeReferenceMapper = typeReferenceMapper;
            _methodUrlMapper = methodUrlMapper;
            _httpMethodMapper = httpMethodMapper;
        }

        public void Read(Assembly assembly, ServicesDefinitions definitions, ApiControllerLoaderOptions options = null) {
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
        public void AddController([NotNull] Type controller, [NotNull] ServicesDefinitions definitions)
        {
            var name = controller.Name.Replace("Controller", "Client");

            var methods = (from method in controller.GetMethods()
                           where !typeof(IActionResult).IsAssignableFrom(method.ReturnType)
                           select method).ToArray();

            if (methods.Length == 0) {
                return;
            }

            definitions.Services.GetOrCreate(name, () =>
            {
                var service = new Service(name);

                foreach (var methodInfo in methods)
                {
                    var method = GetMethod(methodInfo, service, definitions);

                    if (method != null) {
                        service.Methods.Add(method);
                    }
                }

                return service;
            });
        }

        [CanBeNull]
        private ServiceMethod GetMethod([NotNull] MethodInfo methodInfo, [NotNull] Service client, [NotNull] ServicesDefinitions definitions)
        {
            var method = new ServiceMethod(_methodNameMapper.GetMethodName(methodInfo));

            method.SourceMethod = methodInfo;

            method.HttpMethod = _httpMethodMapper.MapHttpMethod(methodInfo);

            method.Url = _methodUrlMapper.MapMethodUrl(methodInfo);

           

            var parameters = ParametersReader.ReadFrom(methodInfo);

            var bodyParameter = parameters.Find(p => p.PassOver == ParameterPassOver.Body);
            if (bodyParameter != null)
            {
                method.Request = new MethodRequest
                {
                    Type = _propertyTypeMapper.GetPropertyType(bodyParameter.Type.GetTypeInfo(), new TypeReferenceContext(serivces.Definitions))
                };
            }

            var queryParameters = parameters.Where(p => p.PassOver == ParameterPassOver.Query).ToArray();
            if (queryParameters.Length > 0)
            {
                var structureParam = Array.Find(queryParameters, p => !p.Type.IsPrimitive && typeof(string) != p.Type);

                if (structureParam != null)
                {
                    method.Url.Parameters = _ty.ConvertType(structureParam.Type.GetTypeInfo(), new TypeContext(serivces.Definitions));
                }
                else {
                    var singleParameters = queryParameters;
                    var name = "I" + client.Name + methodInfo.Name + "Request";
                    method.Url.Parameters.Structure = serivces.Definitions.Interfaces.GetOrCreate(name, () => {
                        var @interface = new Interface(name)
                        {
                            IsExported = true
                        };

                        foreach (var param in singleParameters)
                        {
                            var property = new InterfaceProperty(param.Name)
                            {
                                Type = _propertyTypeMapper.GetPropertyType(param.Type.GetTypeInfo(), new TypeReferenceContext(serivces.Definitions))
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
                    Type = _propertyTypeMapper.GetPropertyType(returnType.GetTypeInfo(), new TypeReferenceContext(serivces.Definitions))
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
