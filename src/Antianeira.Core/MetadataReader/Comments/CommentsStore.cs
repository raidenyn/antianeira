using System;
using System.Collections.Generic;
using System.Reflection;
using Antianeira.Schema;

namespace Antianeira.MetadataReader.Comments
{
    internal class CommentsStore : ICommentsProvider
    {
        public IDictionary<string, Comment> Types = new Dictionary<string, Comment>(0);

        public IDictionary<string, Comment> Properties = new Dictionary<string, Comment>(0);

        public IDictionary<string, Comment> Fields = new Dictionary<string, Comment>(0);

        public IDictionary<string, Comment> Methods = new Dictionary<string, Comment>(0);

        public Comment GetComment(PropertyInfo property)
        {
            if (Properties.TryGetValue(property.DeclaringType.FullName + "." + property.Name, out var comment))
            {
                return comment;
            }
            return null;
        }

        public Comment GetComment(Type type)
        {
            if (Types.TryGetValue(type.FullName, out var comment))
            {
                return comment;
            }
            return null;
        }

        public Comment GetComment(FieldInfo field)
        {
            if (Fields.TryGetValue(field.DeclaringType.FullName + "." + field.Name, out var comment))
            {
                return comment;
            }
            return null;
        }

        public Comment GetComment(MethodInfo method)
        {
            if (Methods.TryGetValue(method.DeclaringType.FullName + "." + method.Name, out var comment))
            {
                return comment;
            }
            return null;
        }
    }
}
