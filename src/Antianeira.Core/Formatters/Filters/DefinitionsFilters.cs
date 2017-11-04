using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class DefinitionsFilters
    {
        public static string Classes(Definitions @definitions)
        {
            var writer = new StringWriter();

            writer.Join("\n\n", @definitions.Classes);

            return writer.ToString();
        }

        public static string Interfaces(Definitions @definitions)
        {
            var writer = new StringWriter();

            writer.Join("\n\n", @definitions.Interfaces);

            return writer.ToString();
        }

        public static string Types(Definitions @definitions)
        {
            var writer = new StringWriter();

            writer.Join("\n\n", @definitions.Types);

            return writer.ToString();
        }

        public static string Enums(Definitions @definitions)
        {
            var writer = new StringWriter();

            writer.Join("\n\n", @definitions.Enums);

            return writer.ToString();
        }
    }
}
