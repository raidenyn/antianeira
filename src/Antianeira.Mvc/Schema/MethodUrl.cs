using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class MethodUrl : Drop, IWritable
    {
        [NotNull, ItemNotNull]
        public IList<IMethodUrlPathSegment> Segments { get; set; } = new List<IMethodUrlPathSegment>();

        [NotNull, ItemNotNull]
        public IList<IMethodUrlQueryItem> QueryItems { get; set; } = new List<IMethodUrlQueryItem>(0);

        public void Write(IWriter writer)
        {
            var @params = writer.Parameters.Get(() => new MethodUrlRenderParams());

            writer.Append($"let {@params.UrlVariableName} = ");

            writer.Append("`/");
            writer.Join("/", Segments);
            writer.Append("`;\n");

            if (QueryItems.Count > 0)
            {
                writer.Append("const queryParameters = new Array<string>();\n");

                foreach (var item in QueryItems.Where(i => i.SourceName == null))
                {
                    writer.Append("queryParameters.push(`");
                    item.Write(writer);
                    writer.Append("`);\n");
                }
                foreach (var item in QueryItems.Where(i => i.SourceName != null))
                {
                    writer.Append($"if ({item.SourceName} != null) {{\n");
                    writer.Append("queryParameters.push(`");
                    item.Write(writer);
                    writer.Append("`);\n}\n");
                }

                writer.Append("const query = queryParameters.join('&');\n");
                writer.Append($"if (query !== '') {{\n{@params.UrlVariableName} += '?' + query;}}");
            }
        }
    }

    public class MethodUrlRenderParams
    {
        public string UrlVariableName { get; set; } = "url";
    }
}
