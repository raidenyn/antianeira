using System;
using Microsoft.Extensions.CommandLineUtils;
using Antianeira.Schema;
using Antianeira.MetadataReader;
using System.Reflection;
using System.IO;
using Antianeira.Formatters;

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

                command.OnExecute(() =>
                {
                    Generate(input.Value, output.Value());
                    return 0;
                });
            });

            app.Execute(args);
        }

        public static void Generate(string assemblyPath, string outputPath) {
            var definitions = new Definitions();

            new ApiControllerLoader().Read(Assembly.LoadFrom(assemblyPath), definitions);

            if (String.IsNullOrEmpty(outputPath)) {
                outputPath = Path.ChangeExtension(assemblyPath, "ts");
            }

            using (var output = new AlignStreamWriter(new FileStream(outputPath, FileMode.Create, FileAccess.Write)))
            {
                FormatterTemplates.Current.Render(output, definitions);
            }
        }
    }
}