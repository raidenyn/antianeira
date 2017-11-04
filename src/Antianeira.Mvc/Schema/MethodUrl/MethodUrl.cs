using System.Collections.Generic;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema.MethodUrl
{
    public class MethodUrl : Drop, IWritable
    {
        [NotNull]
        public IList<IMethodUrlPathSegment> Segments { get; set; } = new List<IMethodUrlPathSegment>(0);

        [NotNull]
        public IList<IMethodUrlQueryItem> QueryItems { get; set; } = new List<IMethodUrlQueryItem>(0);

        public void Write(IWriter writer)
        {
            writer.Append("`/");
            writer.Join("/", Segments);

            if (QueryItems.Count > 0)
            {
                writer.Append("?");
                writer.Join("&", QueryItems);
            }

            writer.Append("`");
        }
    }
}
