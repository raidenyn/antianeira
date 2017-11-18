using System;
using System.Linq;
using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class ServicesFilters
    {
        public static string Clients(ServicesDefinitions definitions)
        {
            var writer = new System.IO.StringWriter();

            foreach (var serivce in definitions.Services)
            {
                FormatterTemplates.Current.Render(writer, serivce);
            }

            return writer.ToString();
        }

        public static string ClientNames(ServicesDefinitions definitions)
        {
            return String.Join(",\n", definitions.Services.Select(c => c.Name));
        }
    }
}
