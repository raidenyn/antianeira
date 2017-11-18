using System;
using System.IO;
using System.Reflection;
using System.Text;
using Antianeira.Formatters;
using Antianeira.MetadataReader;
using Antianeira.Schema;
using Autofac;
using JetBrains.Annotations;

namespace Antianeira
{
    public class GenerateParams
    {
        [NotNull]
        public string AssemblyPath { get; set; }

        [CanBeNull]
        public string OutputPath { get; set; }

        [CanBeNull]
        public ApiControllerLoaderOptions Options { get; set; }
    }

    public class GenerateCommand: Command
    {
        private readonly GenerateParams _params;

        public GenerateCommand(GenerateParams @params)
        {
            _params = @params;
        }

        public override void Execute()
        {
            var container = GetIoCContainer();

            var reader = container.Resolve<IApiControllerReader>();

            var services = new ServicesDefinitions();

            reader.Read(Assembly.LoadFrom(_params.AssemblyPath), services, _params.Options);

            if (String.IsNullOrEmpty(_params.OutputPath))
            {
                _params.OutputPath = Path.ChangeExtension(_params.AssemblyPath, "ts");
            }

            var source = container.Resolve<IFormatterTemplates>().Render(services);
            source = new TypeScriptSimpleFormatter().Format(source);

            var directory = Path.GetDirectoryName(_params.OutputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_params.OutputPath, source, Encoding.UTF8);
        }
    }
}
