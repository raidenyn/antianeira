using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class InterfaceMethodFilters
    {
        public static string Parameters(InterfaceMethod method)
        {
            return FormatterTemplates.Current.RenderMany(method.Parameters, ", ");
        }
    }
}
