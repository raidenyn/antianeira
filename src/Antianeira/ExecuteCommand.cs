using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Antianeira.Formatters;
using Antianeira.MetadataReader;
using Antianeira.Schema;
using Autofac;
using JetBrains.Annotations;

namespace Antianeira
{
    public class ExecuteParams
    {
        [NotNull]
        public string AssemblyPath { get; set; }
    }

    public class ExecuteCommand : Command
    {
        private readonly ExecuteParams _params;

        public ExecuteCommand(ExecuteParams @params)
        {
            _params = @params;
        }

        public override void Execute()
        {
            var builder = GetIoCBuilder();

            var configType = Assembly.LoadFrom(_params.AssemblyPath)
                                     .GetExportedTypes()
                                     .FirstOrDefault(t => typeof(IAntianeiraConfiguration).IsAssignableFrom(t));

            if (configType == null)
            {
                throw new Exception("Configuration class is not found.");
            }

            var configurator = (IAntianeiraConfiguration) Activator.CreateInstance(configType);

            var container = configurator.Configure(builder);

            var execute = container.Resolve<IAntianeiraExecution>();

            execute.Execute();
        }
    }

    public interface IAntianeiraConfiguration
    {
        [NotNull]
        IContainer Configure([NotNull] ContainerBuilder builder);
    }

    public interface IAntianeiraExecution
    {
        void Execute();
    }
}
