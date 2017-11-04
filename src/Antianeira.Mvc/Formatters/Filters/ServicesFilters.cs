using System;
using System.Linq;
using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class ServicesFilters
    {
        public static string Clients(Services services)
        {
            var writer = new System.IO.StringWriter();

            foreach (var client in services.Clients)
            {
                FormatterTemplates.Current.Render(writer, client);
            }

            return writer.ToString();
        }

        public static string Names(Services services)
        {
            var writer = new StringWriter();

            writer.Append(String.Join(",\n", services.Clients.Select(c => c.Name)));

            return writer.ToString();
        }
    }
}
