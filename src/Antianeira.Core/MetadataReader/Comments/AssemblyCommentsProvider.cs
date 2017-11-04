using System;
using System.Collections.Concurrent;
using System.Reflection;
using Antianeira.Schema;

namespace Antianeira.MetadataReader.Comments
{
    internal class AssemblyCommentsProvider : ICommentsProvider
    {
        private readonly ConcurrentDictionary<Assembly, ICommentsProvider> _stores = new ConcurrentDictionary<Assembly, ICommentsProvider>();

        private readonly AssemblyCommentsLoader _loader = new AssemblyCommentsLoader();

        public Comment GetComment(PropertyInfo property)
        {
            return _stores.GetOrAdd(property.DeclaringType.Assembly, _loader.Load).GetComment(property);
        }

        public Comment GetComment(Type type)
        {
            return _stores.GetOrAdd(type.Assembly, _loader.Load).GetComment(type);
        }

        public Comment GetComment(FieldInfo field)
        {
            return _stores.GetOrAdd(field.DeclaringType.Assembly, _loader.Load).GetComment(field);
        }
    }
}
