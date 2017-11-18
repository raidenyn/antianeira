using Antianeira.Schema;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IApiControllerReader
    {
        void Read([NotNull] Assembly assembly, [NotNull] ServicesDefinitions serivces, ApiControllerLoaderOptions options = null);
    }

    public class ApiControllerReader: IApiControllerReader
    {
        private readonly IServiceNameMapper _serviceNameMapper;
        private readonly IControllerChecker _controllerChecker;
        private readonly IActionChecker _actionChecker;
        private readonly IServiceMethodMapper _serviceMethodMapper;

        public ApiControllerReader(
            IServiceNameMapper serviceNameMapper, 
            IControllerChecker controllerChecker, 
            IActionChecker actionChecker, 
            IServiceMethodMapper serviceMethodMapper)
        {
            _serviceNameMapper = serviceNameMapper;
            _controllerChecker = controllerChecker;
            _actionChecker = actionChecker;
            _serviceMethodMapper = serviceMethodMapper;
        }

        public void Read(Assembly assembly, ServicesDefinitions definitions, ApiControllerLoaderOptions options = null) {
            options = options ?? new ApiControllerLoaderOptions();

            var controllers = from type in assembly.GetTypes()
                              where _controllerChecker.IsAvailableContoller(type)
                                    && Glob.Glob.IsMatch(type.FullName, options.TypeFilter)
                              select type;

            foreach (var controller in controllers) {
                TryAddController(controller, definitions);
            }
        }

        public bool TryAddController([NotNull] Type controller, [NotNull] ServicesDefinitions definitions)
        {
            var name = _serviceNameMapper.MapServiceName(controller);

            var methods = (from method in controller.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                           where _actionChecker.IsAvailableAction(method)
                           select method).ToArray();

            if (methods.Length == 0) {
                return false;
            }

            var context = new MethodContexts(definitions);

            definitions.Services.GetOrCreate(name, () =>
            {
                var service = new Service(name);

                foreach (var methodInfo in methods)
                {
                    var method = _serviceMethodMapper.GetMethod(methodInfo, context);

                    if (method != null) {
                        service.Methods.Add(method);
                    }
                }

                return service;
            });

            return true;
        }
    }

    public class ApiControllerLoaderOptions {
        /// <summary>
        /// Glob filter
        /// </summary>
        public string TypeFilter { get; set; } = "*";
    }
}
