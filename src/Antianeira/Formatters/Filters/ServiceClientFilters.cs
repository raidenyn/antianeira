using System;
using Antianeira.Schema;
using System.Linq;
using System.Text;
using Antianeira.Schema.Api;

namespace Antianeira.Formatters.Filters
{
    public static class ServiceClientFilters
    {
        public static string Methods(ServiceClient client)
        {
            return FormatterTemplates.Current.RenderMany(client.Methods);
        }
    }
}
