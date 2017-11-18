using System;
using Microsoft.Extensions.CommandLineUtils;
using Antianeira.MetadataReader;

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
                command.Description = "Generate typescript file.";
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
                    new GenerateCommand(new GenerateParams
                    {
                        AssemblyPath = input.Value,
                        OutputPath = output.Value(),
                        Options = new ApiControllerLoaderOptions
                        {
                            TypeFilter = types.HasValue() ? types.Value() : "*"
                        }
                    }).Execute();

                    return 0;
                });
            });

            app.Command("exec", (command) =>
            {
                command.Description = "Execute configuration from custom assembly.";
                command.HelpOption("-?|-h|--help");

                var input = command.Argument("[input]",
                    "Path to .NET assembly with antianeira configuration.");

                command.OnExecute(() =>
                {
                    new ExecuteCommand(new ExecuteParams
                    {
                        AssemblyPath = input.Value
                    }).Execute();

                    return 0;
                });
            });

            app.Execute(args);
        }
    }
}