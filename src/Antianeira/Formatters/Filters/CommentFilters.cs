using System;
using Antianeira.Schema;

namespace Antianeira.Formatters.Filters
{
    public static class CommentFilters
    {
        public static string Comment(Comment comment)
        {
            if (comment != null)
            {
                return FormatterTemplates.Current.Render(comment);
            }
            return String.Empty;
        }
    }
}
