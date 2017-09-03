using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class EnumFieldFilters
    {
        public static string Field(EnumField field)
        {
            return field.Name;
        }

        public static string Value(EnumField field)
        {
            if (field.IsNumeric) {
                return field.Value;
            }
            return $"'{field.Value}'";
        }
    }
}
