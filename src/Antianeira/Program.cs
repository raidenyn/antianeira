using System;
using Microsoft.Extensions.CommandLineUtils;
using Antianeira.MetadataReader;
using System.Reflection;
using System.IO;
using Antianeira.Schema;
using Antianeira.Formatters;
using Autofac;

namespace Antianeira
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "antianeira";
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                Console.WriteLine("Antianeira bring your MVC controller code definition to your client side!");
                return 0;
            });

            app.Command("generate", (command) =>
            {
                command.Description = "Generate typescript file with Angular 2+ clients.";
                command.HelpOption("-?|-h|--help");

                var input = command.Argument("[input]",
                                             "Path to .NET MVC assembly with controller definitions.");

                var output = command.Option("-o|--output <path>",
                                            "Path to output typescript file.",
                                            CommandOptionType.SingleValue);

                var types = command.Option("--types <types>",
                                           "Glob pattern of namespace",
                                           CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    var options = new ApiControllerLoaderOptions
                    {
                        TypeFilter = types.HasValue() ? types.Value() : "*"
                    };

                    Generate(input.Value, output.Value(), options);
                    return 0;
                });
            });

            app.Execute(args);
        }

        private static IContainer GetIoCContainer()
        {
            var builder = new ContainerBuilder();
            builder.AddMvcReader();
            builder.AddMvcFormatter();

            return builder.Build();
        }

        public static void Generate(string assemblyPath, string outputPath, ApiControllerLoaderOptions options)
        {
            var container = GetIoCContainer();

            var reader = container.Resolve<IApiControllerReader>();

            var services = new Services();

            reader.Read(Assembly.LoadFrom(assemblyPath), services, options);

            if (String.IsNullOrEmpty(outputPath)) {
                outputPath = Path.ChangeExtension(assemblyPath, "ts");
            }

            using (var output = new AlignStreamWriter(new FileStream(outputPath, FileMode.Create, FileAccess.Write)))
            {
                container.Resolve<IFormatterTemplates>().Render(output, services);
            }
        }
    }
}