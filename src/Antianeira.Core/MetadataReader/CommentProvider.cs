using Antianeira.Schema;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface ICommentsProvider
    {
        Comment GetComment(TypeInfo type, PropertyInfo property);

        Comment GetComment(TypeInfo type);

        Comment GetComment(TypeInfo type, FieldInfo field);
    }

    internal class CommentsProvider : ICommentsProvider
    {
        private readonly ConcurrentDictionary<Assembly, ICommentsProvider> _stores = new ConcurrentDictionary<Assembly, ICommentsProvider>();

        private readonly AssemblyCommentsLoader _loader = new AssemblyCommentsLoader();

        public Comment GetComment(TypeInfo type, PropertyInfo property)
        {
            return _stores.GetOrAdd(type.Assembly, _loader.Load).GetComment(type, property);
        }

        public Comment GetComment(TypeInfo type)
        {
            return _stores.GetOrAdd(type.Assembly, _loader.Load).GetComment(type);
        }

        public Comment GetComment(TypeInfo type, FieldInfo field)
        {
            return _stores.GetOrAdd(type.Assembly, _loader.Load).GetComment(type, field);
        }
    }

    internal class CommentsStore : ICommentsProvider
    {
        public IDictionary<string, Comment> Types = new Dictionary<string, Comment>(0);

        public IDictionary<string, Comment> Properties = new Dictionary<string, Comment>(0);

        public IDictionary<string, Comment> Fields = new Dictionary<string, Comment>(0);

        public Comment GetComment(TypeInfo type, PropertyInfo property)
        {
            if (Properties.TryGetValue(type.FullName + "." + property.Name, out var comment))
            {
                return comment;
            }
            return null;
        }

        public Comment GetComment(TypeInfo type)
        {
            if (Types.TryGetValue(type.FullName, out var comment))
            {
                return comment;
            }
            return null;
        }

        public Comment GetComment(TypeInfo type, FieldInfo field)
        {
            if (Fields.TryGetValue(type.FullName + "." + field.Module.Name, out var comment))
            {
                return comment;
            }
            return null;
        }
    }
}
